using AutoMapper;
using CY_BM;
using CY_DM;
using CY_WebApi.DataAccess;
using CY_WebApi.Models;
using CY_WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Repository<CyProduct> _repo;
        private readonly CyContext _db;
        private readonly ExcelReader _excelReader;
        private readonly ILogger<CyProductsController> _logger;

        public CyProductsController(CyContext context, IMapper mapper, ExcelReader excelReader, ILogger<CyProductsController> logger)
        {
            _mapper = mapper;
            //_repo = new Repository<CyProduct>(context);
            _excelReader = excelReader;
            _db = context;
            _logger = logger;
        }

        // GET: api/CyProducts
        [HttpPost("getAllProducts")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetCyProduct(PageDTO PD)
        {
            var skipCount = PD.PageNumber * PD.PageSize;

            var allcount = await _db.CyProduct.Where(c => c.IsVisible).CountAsync();

            //For CHIPYAB
            var resu = await _db.CyProduct.OrderByDescending(c => c.ID).Skip(skipCount).Take(PD.PageSize).ToListAsync();

            //For Sane
            //var resu = await _repo.TableNoTracking.OrderByDescending(c => c.ID).Skip(skipCount).Take(PD.PageSize).ToListAsync();// ; _db.CyProduct.ToListAsync();
            var pagedres = new PageResultDTO<ProductDTO>
            {

                ItemList = resu.Select(c => _mapper.Map<ProductDTO>(c)).ToList(),
                AllCount = allcount,
            };
            return Ok(pagedres);
        }

        // GET: api/CyProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetCyProduct(Guid id)
        {
            var CyProduct = await _db.CyProduct.Where(c => c.ID == id && c.IsVisible).Include(d => d.Specifications).Include(m => m.CyManufacturer).FirstOrDefaultAsync();
            if (CyProduct == null)
            {
                return NotFound();
            }
            var prdDto = _mapper.Map<ProductDTO>(CyProduct);
            if (CyProduct.Specifications != null && CyProduct.Specifications.Count > 0)
            {
                prdDto.Spec = CyProduct.Specifications.Select(c => _mapper.Map<ProductSpecDTO>(c)).ToList();
            }
            prdDto.Manufacturer = CyProduct.CyManufacturer?.Name;
            return Ok(prdDto);
        }

        [HttpGet("prod/{PartNumber}")]
        public async Task<ActionResult<ProductDTO>> GetCyProductByPartNumber(string PartNumber)
        {
            var CyProduct = await _db.CyProduct.Where(c => (c.ProductCode == PartNumber || c.ID.ToString() == PartNumber) &&
                                                            c.IsVisible)
                                                       .Include(m => m.CyManufacturer)
                                                       .FirstOrDefaultAsync();
            if (CyProduct == null)
            {
                return NotFound();
            }
            var prdDto = _mapper.Map<ProductDTO>(CyProduct);
            prdDto.Manufacturer = CyProduct.CyManufacturer?.Name;
            return Ok(prdDto);
        }

        [HttpGet("prod/spec/{id}")]
        public async Task<ActionResult<List<ProductSpecDTO>>> GetProductSpec(Guid id)
        {
            var prodSpec = await _db.CyProductSpec.Where(spec => spec.IsVisible && spec.CyProductId == id)
                                                  .Select(spec => new ProductSpecDTO()
                                                  {
                                                      Name = spec.Name,
                                                      Value = spec.Value,
                                                      CyProductId = id
                                                  })
                                                  .ToListAsync();
            return Ok(prodSpec);
        }

        [HttpPost("SearchProducts")]
        public async Task<IActionResult> SearchProductByAdmin(ProdcutSearchDTO input)
        {

            if (input.PageSize < 1)
            {
                return BadRequest("تعداد نتایج در هر صفحه را وارد کنید");
            }
            var skipCount = input.PageNumber * input.PageSize;

            var Query = _db.CyProduct.Where(c => c.IsVisible);

            if (input.Name != null)
            {   //For Chipyab
                Query = Query.Where(c => c.Name.StartsWith(input.Name));

                //For Sane
                //Query = Query.Where(c => c.Name.Contains(input.Name));
            }

            if (input.CategoryCode != null)
            {
                input.CategoryCode = input.CategoryCode.Trim();
                Query = Query.Where(c => c.CyCategory.Code == input.CategoryCode);
            }

            if (input.ProductCategoryId != null)
            {
                Query = Query.Where(c => c.CyProductCategoryId == input.ProductCategoryId);
            }
            else if (input.ProductCategoryCode != null)
            {
                input.ProductCategoryCode = input.ProductCategoryCode.Trim();
                Query = Query.Where(c => c.CyProductCategory.Code == input.ProductCategoryCode || c.CyProductCategory.Url == input.ProductCategoryCode);
            }

            if (input.ManufacturerName != null)
            {
                input.ManufacturerName = input.ManufacturerName.Trim();
                Query = Query.Where(c => c.CyManufacturer.Code == input.ManufacturerName);
            }

            var products = await Query.Skip(skipCount).Take(input.PageSize).ToListAsync();
            var resu = _mapper.Map<List<ProductDTO>>(products);
            var pagedres = new PageResultDTO<ProductDTO>
            {

                ItemList = resu.Select(c => _mapper.Map<ProductDTO>(c)).ToList(),
                AllCount = Query.Count(),
            };
            return Ok(pagedres);
        }

        //[HttpPut("{id}")]
        ////[TypeAuthorize([UserType.SalesManager, UserType.WarehouseKeeper])]
        //public async Task<IActionResult> PutCyProduct(Guid id, ProductDTO product)
        //{
        //    var CyProduct = _mapper.Map<CyProduct>(product);

        //    if (id != CyProduct.ID)
        //    {
        //        return BadRequest("Not Valid ID");
        //    }
        //    try
        //    {
        //        int cg1 = await _repo.Update(CyProduct);//  _db.SaveChangesAsync();
        //        if (product.Spec != null && product.Spec.Count > 0)
        //        {
        //            var spec = await _db.CyProductSpec.Where(c => c.CyProductId == id).ToListAsync();
        //            var newSpec = product.Spec;
        //            var removeItem = new List<CyProductSpec>();
        //            foreach (var item in spec)
        //            {
        //                var fItem = newSpec.Find(c => c.ID == item.ID);
        //                if (fItem != null)
        //                {
        //                    item.Value = fItem.Value;
        //                    item.Name = fItem.Name;
        //                }
        //                else
        //                {
        //                    removeItem.Add(item);
        //                }
        //            }
        //            var newItem = newSpec.Where(c => c.ID < 1).ToList();
        //            if (newItem != null && newItem.Count > 0)
        //            {
        //                _db.CyProductSpec.AddRange(newItem.Select(c => _mapper.Map<CyProductSpec>(c)).ToList());
        //            }
        //            if (removeItem.Count > 0)
        //            {
        //                _db.CyProductSpec.RemoveRange(removeItem);
        //            }
        //            Ok(new { resu = await _db.SaveChangesAsync() + cg1 });
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //    return NoContent();
        //}

        // POST: api/CyProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[TypeAuthorize([UserType.SalesManager, UserType.WarehouseKeeper])]
        public async Task<ActionResult<ProductDTO>> PostCyProduct(ProductDTO ProductDTO)
        {
            var CyProduct = _mapper.Map<CyProduct>(ProductDTO);

            if (ProductDTO.Spec != null && ProductDTO.Spec.Count > 0)
            {
                var cySpec = ProductDTO.Spec.Select(c => _mapper.Map<CyProductSpec>(c)).ToList();
                CyProduct.Specifications = cySpec;
            }

            CyProduct = await _repo.Insert(CyProduct);

            return CreatedAtAction("GetCyProduct", new { id = CyProduct.ID }, _mapper.Map<ProductDTO>(CyProduct));
        }

        [HttpPost("UpdateByExcel")]
        //[TypeAuthorize([UserType.SalesManager, UserType.WarehouseKeeper])]
        public async Task<IActionResult> UpdateByExcel(Guid input)
        {
            //var identity = User.Identities.FirstOrDefault();
            //if (identity == null)
            //    return Unauthorized("کاربر پیدا نشد");
            //var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var selFile = await _db.CyFile.Where(f => f.FileIQ == input && f.IsVisible).FirstOrDefaultAsync();
            if (selFile != null)
            {
                var BaseFolderPath = Path.Combine("FileContents", "private", selFile.FolderName);
                var RelatedAddress = Path.Combine(BaseFolderPath, selFile.FileIQ + selFile.FileType);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), RelatedAddress);

                if (!System.IO.File.Exists(fullPath))
                {
                    return BadRequest("File not Exist!!");
                }

                var fileItems = _excelReader.ReadExcelForUpdating(fullPath);
                foreach (var partInfo in fileItems)
                {
                    partInfo.PartNumber = partInfo.PartNumber?.Trim();
                    var product = await _db.CyProduct.FirstOrDefaultAsync(p => p.IsVisible && (p.PartNo == partInfo.PartNumber ||
                                                                                  p.Name == partInfo.PartNumber
                                                                                  ));
                    if (product != null)
                    {
                        product.Supply = partInfo.Quantity;
                        product.NoOffPrice = partInfo.Price;
                        product.Price = partInfo.Price;
                    }
                    else
                    {
                        if (partInfo.PartNumber != null)
                        {
                            var newProduct = new CyProduct()
                            {
                                Name = partInfo.PartNumber,
                                PartNo = partInfo.PartNumber,
                                ProductCode = partInfo.PartNumber,
                                NoOffPrice = partInfo.Price,
                                //For Sane only:
                                //Price = partInfo.Price,


                                Supply = partInfo.Quantity
                            };
                            await _repo.Insert(newProduct);
                        }
                    }
                }
                return Ok(await _repo.SaveChangesAsync());
            }
            else
            {
                return BadRequest("no such file exists");
            }

        }


        [HttpPost("UpdateByExcelV2")]
        //[TypeAuthorize([UserType.SalesManager, UserType.WarehouseKeeper])]
        public async Task<IActionResult> UpdateByExcelV2(Guid input)
        {
            //var identity = User.Identities.FirstOrDefault();
            //if (identity == null)
            //    return Unauthorized("کاربر پیدا نشد");
            //var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var selFile = await _db.CyFile.Where(f => f.FileIQ == input && f.IsVisible).FirstOrDefaultAsync();
            if (selFile != null)
            {
                var BaseFolderPath = Path.Combine("FileContents", "private", selFile.FolderName);
                var RelatedAddress = Path.Combine(BaseFolderPath, selFile.FileIQ + selFile.FileType);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), RelatedAddress);

                if (!System.IO.File.Exists(fullPath))
                {
                    return BadRequest("File not Exist!!");
                }

                var fileItems = _excelReader.ReadExcelForUpdating(fullPath);

                var manufacturers = await _db.CyManufacturer.Where(m => m.IsVisible && fileItems.Any(i => i.Manufacturer == m.Name)).ToListAsync();
                var products = await _repo.Table.Where(p => p.IsVisible && fileItems.Any(i => i.PartNumber == p.PartNo)).Include(x => x.CyManufacturer).ToListAsync();
                //var products = await _repo.TableNoTracking.Include(p => p.CyManufacturer).ToListAsync();
                //var manufactures = await _db.CyManufacturer.Where(m => m.IsVisible).ToListAsync();
                foreach (var partInfo in fileItems)
                {

                    partInfo.PartNumber = partInfo.PartNumber?.Trim();
                    var product = products.FirstOrDefault(p => p.IsVisible && (p.PartNo == partInfo.PartNumber ||
                                                                                  p.Name == partInfo.PartNumber
                                                                                  ));
                    var manufacturer = manufacturers.FirstOrDefault(m => m.Name == partInfo.Manufacturer);
                    if (product != null)
                    {
                        product.Supply = partInfo.Quantity;
                        product.NoOffPrice = partInfo.Price;
                        product.Price = partInfo.Price;
                    }
                    else
                    {
                        var newProduct = new CyProduct()
                        {
                            Name = partInfo.PartNumber,
                            PartNo = partInfo.PartNumber,
                            ProductCode = partInfo.PartNumber,
                            NoOffPrice = partInfo.Price,
                            //For Sane only:
                            //Price = partInfo.Price,

                            Supply = partInfo.Quantity
                        };
                        if (!string.IsNullOrEmpty(partInfo.Manufacturer))
                        {
                            //var manufacturer = manufactures.FirstOrDefault(m => m.Name == partInfo.Manufacturer);
                            if (manufacturer == null)
                            {
                                CyManufacturer entity = new CyManufacturer()
                                {
                                    Name = partInfo.Manufacturer,
                                    Code = partInfo.Manufacturer.Replace("/", "_").Replace(" ", ""),
                                    IsVisible = true
                                };
                                var insertedManufacturer = _db.CyManufacturer.Add(entity);
                                newProduct.CyManufacturer = insertedManufacturer.Entity;
                            }
                            else
                            {
                                newProduct.CyManufacturer = manufacturer;
                            }
                        }
                        _repo.InsertWithoutSave(newProduct);
                    }
                }
                return Ok(await _db.SaveChangesAsync());
            }
            else
            {
                return BadRequest("no such file exists");
            }
        }
        //[HttpPut("UpdateExcelForSane")]
        //public async Task<ActionResult> updateSane(Guid guid, int id)
        //{
        //    var selFile = await _db.CyFile.Where(f => f.FileIQ == guid && f.IsVisible).FirstOrDefaultAsync();
        //    if (selFile != null)
        //    {
        //        var BaseFolderPath = Path.Combine("FileContents", "private", selFile.FolderName);
        //        var RelatedAddress = Path.Combine(BaseFolderPath, selFile.FileIQ + selFile.FileType);
        //        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), RelatedAddress);

        //        if (!System.IO.File.Exists(fullPath))
        //        {
        //            return BadRequest("File not Exist!!");
        //        }

        //        var fileItems = _excelReader.ReadExcelForUpdating(fullPath);
        //        var fileItemNames = fileItems.Select(c => c.PartNumber?.Replace(" ","")).ToList();

        //        var SpecCategoryProducts = await _repo.Table.Where(p => p.IsVisible && p.CyCategoryId == id).Select(p => p.PartNo.Replace(" ","")).ToListAsync();

        //        var allProducts = await _repo.Table.Where(p => p.IsVisible).ToListAsync();

        //        foreach (var product in allProducts)
        //        {
        //            if (!fileItemNames.Contains(product.PartNo?.Replace(" ","")) && !SpecCategoryProducts.Contains(product.PartNo.Replace(" ","")))
        //            {
        //                product.Supply = 0;
        //            }
        //        }
        //        Ok(await _repo.SaveChangesAsync());
        //    }
        //    return BadRequest();
        //}

        [Route("GetProductByCat")]
        [HttpPost]
        public async Task<ActionResult<PageResultDTO<ProductDTO>>> GetProductByCat(PageDTO PD)
        {
            var skipCount = PD.PageNumber * PD.PageSize;

            var allcount = await _repo.Table.Where(c => c.CyCategoryId.HasValue && c.CyCategory.Code == PD.Cat
             && c.IsVisible).CountAsync();

            var ProdList = await _repo.Table.Where(c => c.CyCategoryId.HasValue && c.CyCategory.Code == PD.Cat
            && c.IsVisible)
                .OrderBy(c => c.CreateDate).Skip(skipCount).Take(PD.PageSize).ToListAsync();
            if (ProdList != null)
            {
                var menuWitems = new PageResultDTO<ProductDTO>
                {
                    PageTitle = ProdList[0]?.CyCategory?.Text,
                    ItemList = ProdList.Select(c => _mapper.Map<ProductDTO>(c)).ToList(),
                    AllCount = allcount,
                };
                return Ok(menuWitems);
            }
            return NotFound();
        }

        [Route("GetProductByProductCat")]
        [HttpPost]
        public async Task<ActionResult<PageResultDTO<ProductDTO>>> GetProductByProdCat(PageDTO PD)
        {
            var skipCount = PD.PageNumber * PD.PageSize;

            var allcount = await _repo.Table.Where(c => c.CyProductCategoryId.HasValue && c.CyProductCategory.Code == PD.Cat
             && c.IsVisible).CountAsync();

            var ProdList = await _repo.Table.Where(c => c.CyProductCategoryId.HasValue && c.CyProductCategory.Code == PD.Cat
            && c.IsVisible)
                .OrderBy(c => c.CreateDate).Skip(skipCount).Take(PD.PageSize).ToListAsync();
            if (ProdList.Count > 0)
            {
                var menuWitems = new PageResultDTO<ProductDTO>
                {
                    //PageTitle = ProdList[0]?.CyCategory?.Text,
                    ItemList = ProdList.Select(c => _mapper.Map<ProductDTO>(c)).ToList(),
                    AllCount = allcount,
                };
                return Ok(menuWitems);
            }
            return NotFound();
        }

        //[TypeAuthorize([UserType.SalesManager, UserType.Reporter, UserType.WarehouseKeeper])]
        [HttpPost("productsWithNull")]
        public async Task<ActionResult<PageResultDTO<ProductDTO>>> getProductWithNull(PageDTOprodWithNull PD)
        {
            var skipCount = PD.PageNumber * PD.PageSize;
            int AllCount = 0;
            List<ProductDTO> result;
            if (PD.Property == ProductProperty.manufacturer)
            {
                AllCount = await _repo.TableNoTracking.Where(p => p.IsVisible && p.CyManufacturerId == null).CountAsync();
                result = await _repo.TableNoTracking.Where(p => p.IsVisible && p.CyManufacturerId == null).Skip(skipCount).Take(PD.PageSize).Select(p => _mapper.Map<ProductDTO>(p)).ToListAsync();
            }
            else if (PD.Property == ProductProperty.category)
            {
                AllCount = await _repo.TableNoTracking.Where(p => p.IsVisible && (p.CyProductCategoryId == null || p.CyCategoryId == null)).CountAsync();
                result = await _repo.TableNoTracking.Where(p => p.IsVisible && (p.CyProductCategoryId == null || p.CyCategoryId == null)).Skip(skipCount).Take(PD.PageSize).Select(p => _mapper.Map<ProductDTO>(p)).ToListAsync();
            }
            else
            {
                AllCount = await _repo.TableNoTracking.Where(p => p.IsVisible && p.SmallImage == null).Select(p => _mapper.Map<ProductDTO>(p)).CountAsync();
                result = await _repo.TableNoTracking.Where(p => p.IsVisible && p.SmallImage == null).Skip(skipCount).Take(PD.PageSize).Select(p => _mapper.Map<ProductDTO>(p)).ToListAsync();
            }
            var pagedRes = new PageResultDTO<ProductDTO>()
            {
                ItemList = result,
                AllCount = AllCount,
            };
            return Ok(pagedRes);
        }

        // DELETE: api/CyProducts/5
        //[TypeAuthorize([UserType.SalesManager, UserType.WarehouseKeeper])]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCyProduct(Guid id)
        {
            await _repo.Delete(id);
            return NoContent();
        }
    }
}
