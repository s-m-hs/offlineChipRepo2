using AutoMapper;
using AutoMapper.Configuration.Annotations;
using ClosedXML.Excel;
using CY_BM;
using CY_DM;
using CY_WebApi.Models;
using CY_WebApi.Services;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyProductsBController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CyContext _db;

        public CyProductsBController(CyContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }


        //[HttpGet("searchProduct")]
        //public  List<CyProduct> SearchByPartNo(string search)

        //{
        //    return _db.CyProduct
        //        .FromSqlRaw(
        //            @"SELECT * FROM CyProduct 
        //      WHERE CONTAINS(PartNo, @search)",
        //            new SqlParameter("@search", $"\"*{search}*\"")
        //        )
        //        .AsNoTracking()
        //        .ToList();


        //}

        [HttpGet("searchProduct")]
        public async Task<ActionResult<ProductPageSelect>> SearchProducts([FromQuery] ProductSearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Search))
                return BadRequest("Search term is required.");

            // محتوای پارامتر جستجو برای Full-Text
            var searchParam = new SqlParameter("@search", $"\"*{request.Search}*\"");

            // پایه کوئری
            var query = _db.CyProduct
                .FromSqlRaw(
                    @"SELECT * FROM CyProduct 
              WHERE CONTAINS((PartNo), @search)",
                    searchParam
                )
                .AsNoTracking();

            // مرتب‌سازی داینامیک
            query = request.SortBy.ToLower() switch
            {
                "name" => request.Descending ? query.OrderByDescending(p => p.Name)
                                             : query.OrderBy(p => p.Name),
                "id" => request.Descending ? query.OrderByDescending(p => p.ID)
                                           : query.OrderBy(p => p.ID),
                _ => query.OrderBy(p => p.ID)
            };

            // Pagination
            int skip = (request.Page - 1) * request.PageSize;
            var products = await query.Include(u => u.CyProductCategory).Include(u => u.CyManufacturer)
                .Skip(skip)
                .Take(request.PageSize)
                .Select(s => new ProductSelect
                {
                    id = s.ID,
                    partNo = s.PartNo,
                    cyProductCategory = s.CyProductCategory.Name,
                    cyManufacturer = s.CyManufacturer.Name,
                    description = s.Description,
                    price = s.Price,
                    supply = s.Supply,
                    smallImage = s.SmallImage
                })
                .ToListAsync();

            // Total Count (اختیاری برای Frontend)
            var totalCount = await _db.CyProduct
                .FromSqlRaw(
                    @"SELECT * FROM CyProduct 
              WHERE CONTAINS((PartNo), @search)",
                    searchParam
                )
                .CountAsync();

            return Ok(new ProductPageSelect()
            {
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                Products = products
            });
        }


        /// <summary>
        /// آپلود نمونه اکسل برای ثبت سفارش جدید
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("updateSupplyPriceByExell")]
        public async Task<ActionResult> updateSupplyPriceByExell(Guid input)
        {
            var selFile = await _db.CyFile.Where(f => f.FileIQ == input && f.IsVisible).FirstOrDefaultAsync();
            if (selFile == null)
                return BadRequest("no such file exists");

            var baseFolderPath = Path.Combine("FileContents", "private", selFile.FolderName);
            var relatedAddress = Path.Combine(baseFolderPath, selFile.FileIQ + selFile.FileType);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), relatedAddress);


            if (!System.IO.File.Exists(fullPath))
                return BadRequest("File not Exist!!");


            var partList = new List<listPro>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    // Assuming the data is in the first sheet
                    var table = result.Tables[0];

                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        var PartNo = table.Rows[i][0].ToString();
                        var Manufacturer = table.Rows[i][1].ToString();
                        var CreatorComment = table.Rows[i][2].ToString();
                        var Quntity = table.Rows[i][3].ToString();

                        int qu = 0;

                        if (PartNo != null)
                        {
                            if (!string.IsNullOrEmpty(Quntity))
                            {
                                int.TryParse(Quntity, out int quntiti);
                                qu = quntiti;
                            }
                            partList.Add(new listPro
                            {
                                PartNumber = PartNo,
                                Manufacturer = Manufacturer,
                                creatorCommentA = CreatorComment,
                                Quantity = qu
                            });
                        }
                    }
                }
            }




            //foreach (var item in partList)
            //{
            //    var product = _db.CyProduct.Where(x => x.IsVisible && x.CyProductCategoryId != null && x.ID == item.Id).FirstOrDefault();
            //    if (product == null) return BadRequest(new { msg = "this product notFaound", product = product.Name });
            //    var baesPrice = item.ShopPrice;
            //    product.Supply = item.Supply;
            //    product.ShopPrice = item.ShopPrice;
            //    product.Price = baesPrice + (baesPrice * 40 / 100);   ///15-2   20-3 30-4   40-1    
            //    product.Price2 = baesPrice + (baesPrice * 15 / 100);   ///15-2   20-3 30-4   40-1    
            //    product.Price3 = baesPrice + (baesPrice * 20 / 100);   ///15-2   20-3 30-4   40-1    
            //    product.Price4 = baesPrice + (baesPrice * 30 / 100);   ///15-2   20-3 30-4   40-1    

            //}

            //await _db.SaveChangesAsync();


            return Ok(new { partList = partList, ListCount = partList.Count() });
        }



        /// <summary>
        /// آپدیت سفارش توسط فایل اکسل
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("updateOrderByExell")]
        public async Task<ActionResult> updateOrderByExell(Guid input,Guid orderId)
        {
            var Order=await _db.CyOrder.Where(x=>x.IsVisible && x.ID == orderId).Include(i=>i.CyOrderItems).FirstOrDefaultAsync();
            if (Order == null) return BadRequest(new { msg = "order not founf" });

            var selFile = await _db.CyFile.Where(f => f.FileIQ == input && f.IsVisible).FirstOrDefaultAsync();
            if (selFile == null)
                return BadRequest("no such file exists");

            var baseFolderPath = Path.Combine("FileContents", "private", selFile.FolderName);
            var relatedAddress = Path.Combine(baseFolderPath, selFile.FileIQ + selFile.FileType);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), relatedAddress);

            double orderTotalPrice=0;

            if (!System.IO.File.Exists(fullPath))
                return BadRequest("File not Exist!!");


            var partList = new List<listPro>();
            

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    // Assuming the data is in the first sheet
                    var table = result.Tables[0];

                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        var ID = table.Rows[i][0].ToString();
                        var PartNumber = table.Rows[i][1].ToString();
                        var Quantity = table.Rows[i][2].ToString();
                        var UnitPrice = table.Rows[i][3].ToString();
                        var Duration = table.Rows[i][4].ToString();
                        var CommentB = table.Rows[i][5].ToString();

                        int qu = 0;

                        if (ID != null && Guid.TryParse(ID,out Guid IDD) && int.TryParse(Quantity,out int quantity) && double.TryParse(UnitPrice, out double unitPrice))
                        {
                                        var currentOrder = await _db.CyOrder.Where(x=>x.IsVisible && x.ID == orderId ).Include(i=>i.CyOrderItems).FirstOrDefaultAsync();

                            var currentOrderItem=currentOrder.CyOrderItems.Where(x=>x.ID== IDD).FirstOrDefault();
                            if (currentOrderItem == null) return BadRequest(new {msg="فایل بارگزاری شده با سفارش مورد نظر مطابقت ندارد", itemId=IDD});

                            currentOrderItem.Quantity = quantity;
                            currentOrderItem.UnitPrice = unitPrice;
                            currentOrderItem.TotalPrice = Math.Round(quantity * unitPrice, 3); ///محاسبه تا سه رقم اعشار
                            currentOrderItem.Duration = Duration;
                            currentOrderItem.CommentB = CommentB;

                            orderTotalPrice += ((double) currentOrderItem.TotalPrice);

                        }
                    }
                }
            }
            Order.TotalAmount = orderTotalPrice;

            await _db.SaveChangesAsync();



            return Ok(new { partList = partList, ListCount = partList.Count(), totalAmount=orderTotalPrice });
        }

        [HttpGet("downloadExell")]
        public async Task<IActionResult> downloadExell(Guid orderId)
        {
            var order = await _db.CyOrder
                .Where(x => x.IsVisible && x.ID == orderId)
                .Include(x => x.CyOrderItems)
                .FirstOrDefaultAsync();

            if (order == null)
                return NotFound("Order not found");

            var items = order.CyOrderItems;

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("OrderItems");

            // ===== Header =====
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "PartNumber";
            worksheet.Cell(1, 3).Value = "Quantity";
            worksheet.Cell(1, 4).Value = "UnitPrice";
            worksheet.Cell(1, 5).Value = "Duration";
            worksheet.Cell(1, 6).Value = "CommentB";

            // Bold Header
            worksheet.Range(1, 1, 1, 6).Style.Font.Bold = true;

            // ===== Data =====
            int row = 2;
            foreach (var item in items)

            {
                string idString = item.ID.ToString();

                worksheet.Cell(row, 1).Value = idString;
                worksheet.Cell(row, 2).Value = item.PartNumber;
                worksheet.Cell(row, 3).Value = item.Quantity;
                worksheet.Cell(row, 4).Value = item.UnitPrice;
                worksheet.Cell(row, 5).Value = item.Duration;
                worksheet.Cell(row, 6).Value = item.CommentB;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            // ===== Export =====
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Order_{orderId}.xlsx"
            );
        }







    }
}
