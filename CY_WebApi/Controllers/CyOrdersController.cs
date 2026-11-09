//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using CY_DM;
//using CY_BM;
//using CY_WebApi.Models;
//using AutoMapper;
//using CY_WebApi.DataAccess;
//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;
//using CY_WebApi.Services;
//using System.Linq.Dynamic.Core;
//using NuGet.Protocol;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CyOrdersController : ControllerBase
//    {
//        private readonly IMapper _mapper;
//        private readonly Repository<CyOrder> _repo;
//        private readonly Repository<CyOrderItem> _itemRepo;
//        private readonly Repository<CyInspectionForm> _inspectionRepo;
//        private readonly Repository<CyProduct> _productRepo;
//        private readonly Repository<CyAddress> _addressRepo;
//        private readonly Repository<CyPcbForm> _pcbFormRepo;
//        private readonly Repository<CyInspectionItem> _inspectionItemRepo;
//        private readonly Repository<CyUser> _userRepo;
//        private readonly CyContext _db;
//        private readonly ExcelReader _excelReader;
//        private readonly ILogger<CyOrdersController> _logger;
//        private readonly SmsService _smsService;

//        public CyOrdersController(CyContext context, IMapper mapper, ExcelReader excelReader, ILogger<CyOrdersController> logger, SmsService smsService)
//        {
//            _mapper = mapper;
//            _repo = new Repository<CyOrder>(context);
//            _itemRepo = new Repository<CyOrderItem>(context);
//            _addressRepo = new Repository<CyAddress>(context);
//            _inspectionRepo = new Repository<CyInspectionForm>(context);
//            _inspectionItemRepo = new Repository<CyInspectionItem>(context);
//            _pcbFormRepo = new Repository<CyPcbForm>(context);
//            _productRepo = new Repository<CyProduct> (context);
//            _userRepo = new Repository<CyUser> (context);
//            _db = context;
//            _excelReader = excelReader;
//            _logger = logger;
//            _smsService = smsService;
//        }

//        // GET: api/CyOrder/5
//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("GetBasketForUser")]
//        public async Task<ActionResult<OrderReturnDTO>> GetBasket()
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);



//            var cyOrder = await _db.CyOrder.Where(o => o.IsVisible &&
//                                                  o.CyUserID == userid &&
//                                                  o.Status == OrderStatus.InBasket)
//                                            .FirstOrDefaultAsync();

//            if (cyOrder != null)
//            {
//                var OrderProducts = await _db.CyOrderItem.Where(i => i.IsVisible
//                                                         && i.CyOrderID == cyOrder.ID)
//                                                   .Include(p=> p.CyProduct.CyManufacturer)
//                                                   .Select(c => new OrderItemReturnDTO()
//                                                  {
//                                                      ID = c.ID,
//                                                      CyProductID = c.CyProductID,
//                                                      //CyProductName =c.CyProductID!=null? c.CyProduct.Name:"",
//                                                      Status = c.Status,
//                                                      CyProductImgUrl = c.CyProduct != null ? c.CyProduct.SmallImage : null,
//                                                      CyOrderID = c.CyOrderID,
//                                                      UnitPrice = c.UnitPrice,//c.CyProduct != null ? c.CyProduct.Price : null,
//                                                      Quantity = c.Quantity,
//                                                      Manufacturer = c.CyProduct != null ? c.CyProduct.CyManufacturer.Name : c.Manufacturer,
//                                                      PartNumber = c.PartNumber,
//                                                      TotalPrice = (c.UnitPrice * c.Quantity),
//                                                      ProductCode = c.CyProduct != null ? c.CyProduct.ProductCode : null
//                                                  }).Distinct().ToListAsync();
//                if (OrderProducts.Count > 0)
//                {
//                    cyOrder.TotalAmount = OrderProducts.Sum(c => c.Quantity * c.UnitPrice);
//                    await _db.SaveChangesAsync();
//                    var mappedOrder = _mapper.Map<OrderReturnDTO>(cyOrder);
//                    mappedOrder.CyOrderItems = OrderProducts;
//                    return Ok(mappedOrder);
//                }
//                else
//                {
//                    cyOrder.IsVisible = false;
//                    await _db.SaveChangesAsync();
//                }
//            }
//            return Ok(new List<OrderItemReturnDTO>());
//        }


//        // PUT: api/CyOrders/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [TypeAuthorize([UserType.Customer])]
//        [HttpPut("UpdateBasket")]
//        public async Task<IActionResult> UpdateBasket(bool? isOnLogin, ICollection<AddOrderItemDTO> OrderItems)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var userBasket = await _db.CyOrder.Where(o => o.IsVisible &&
//                                                          o.CyUserID == userid &&
//                                                          o.Status == OrderStatus.InBasket)
//                                              .Include(o => o.CyOrderItems.Where(i => i.IsVisible))
//                                              .FirstOrDefaultAsync();

//            if (userBasket == null)
//            {
//                userBasket = new CyOrder()
//                {
//                    Status = OrderStatus.InBasket,
//                    CyUserID = userid,
//                    OrderMode = Ordermode.Normal,
//                    CyOrderItems = new List<CyOrderItem>()
//                };
//                userBasket = await _repo.Insert(userBasket);
//            }


//            foreach (var item in OrderItems)
//            {
//                CyOrderItem? orderItem = null;
//                CyProduct? productItem = null;
//                if (item.CyProductID != null)
//                {
//                    orderItem = userBasket.CyOrderItems.Where(o => o.IsVisible && o.CyProductID == item.CyProductID).FirstOrDefault();
//                    productItem = await _db.CyProduct.Where(p => p.IsVisible && p.ID == item.CyProductID).Include(m => m.CyManufacturer).FirstOrDefaultAsync(); 
//                }
//                else
//                {
//                    orderItem = userBasket.CyOrderItems.Where(o => o.IsVisible && o.ID == item.OrderItemID).FirstOrDefault();
//                }

//                if (orderItem == null)
//                {
//                    if (isOnLogin != null && isOnLogin.Value)
//                    {
//                        //if (item.OrderItemID != null || item.CyProductID != null)
//                        //{
//                            orderItem = new CyOrderItem()
//                            {
//                                CyProductID = item.CyProductID,
//                                Quantity = item.Quantity,
//                                CyOrderID = userBasket.ID,
//                                Manufacturer = productItem != null ? productItem.CyManufacturer?.Name : orderItem?.Manufacturer,
//                                PartNumber = productItem != null ? productItem.PartNo : orderItem?.PartNumber,
//                                UnitPrice = productItem != null ? productItem.Price : null,
//                                TotalPrice = productItem != null ? (productItem.Price * item.Quantity) : 0,
//                            };

//                            await _itemRepo.Insert(orderItem);
//                       // } 
//                    }
//                }
//                else
//                {
//                    orderItem.Quantity = item.Quantity;
//                    orderItem.TotalPrice = (item.Quantity * productItem?.Price);
//                }
//            }
//            userBasket.TotalAmount = userBasket.CyOrderItems.Sum(c => (c.UnitPrice * c.Quantity));
//            await _db.SaveChangesAsync();
//            return Ok("سبد خرید بروزرسانی شد");
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpDelete("deleteBasketForUser")]
//        public async Task<ActionResult> DeleteBasket()
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var userBasketID = await _repo.Table.Where(o => o.IsVisible &&
//                                              o.CyUserID == userid &&
//                                              o.Status == OrderStatus.InBasket)
//                                                .Select(b=> b.ID)
//                                  .FirstOrDefaultAsync();
            
//            await _repo.Delete(userBasketID);
//            return Ok(new { msg = "سبد خرید حذف شد"});
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpDelete("deleteItem/{id}")]
//        public async Task<IActionResult> DeleteOrderItem(int id)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var toBeDeleted = await _itemRepo.Table.Where(i => i.IsVisible &&
//                                                          i.ID == id && i.CyOrder.CyUserID == userid &&
//                                                          (i.CyOrder.Status == OrderStatus.Finalized ||
//                                                           i.CyOrder.Status == OrderStatus.InBasket ||
//                                                           i.CyOrder.Status == OrderStatus.InBOM)).FirstOrDefaultAsync();
//            if (toBeDeleted == null)
//                return NotFound("آیتم پیدا نشد یا کاربر اجازه حذف ندارد");

//            await _itemRepo.Delete(id);
            
//            return Ok(toBeDeleted);
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPost("addToBasket")]
//        public async Task<IActionResult> addToBasket(AddOrderItemDTO input)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            if (input.Quantity == 0) return BadRequest();

//            var prod = await _db.CyProduct.Where(p => p.IsVisible && p.ID == input.CyProductID).FirstOrDefaultAsync();

//            if (prod == null)
//                return NotFound("این محصول وجود ندارد");

//            var userBasket = await _db.CyOrder.Where(o => o.IsVisible &&
//                                                          o.CyUserID == userid &&
//                                                          o.Status == OrderStatus.InBasket)
//                                              .Include(o => o.CyOrderItems.Where(i => i.IsVisible))
//                                              .FirstOrDefaultAsync();

//            if (userBasket == null)
//            {
//                CyOrder newBasket = new CyOrder()
//                {
//                    Status = OrderStatus.InBasket,
//                    CyUserID = userid,
//                    OrderMode = Ordermode.Normal
//                };
//                newBasket = await _repo.Insert(newBasket);


//                var newOItem = new CyOrderItem()
//                {
//                    CyOrderID = newBasket.ID,
//                    CyProductID = input.CyProductID,
//                    Quantity = input.Quantity,
//                    PartNumber = prod.PartNo,
//                    Manufacturer = prod.CyManufacturer?.Name,
//                    UnitPrice = prod.Price,
//                    TotalPrice = (prod.Price * input.Quantity),
//                };
//                await _itemRepo.Insert(newOItem);

//                return Ok("به سبد خرید اضافه شد");
//            }

//            else
//            {
//                var fitem = userBasket.CyOrderItems.Where(o => o.CyProductID == input.CyProductID).FirstOrDefault();
//                if (fitem == null)
//                {
//                    fitem = new CyOrderItem()
//                    {
//                        CyProductID = input.CyProductID,
//                        Quantity = input.Quantity,
//                        CyOrderID = userBasket.ID,
//                        PartNumber = prod.PartNo,
//                        Manufacturer = prod.CyManufacturer?.Name,
//                        UnitPrice = prod.Price,
//                        TotalPrice = (prod.Price * input.Quantity),
//                    };
//                    await _itemRepo.Insert(fitem);
//                    return Ok("محصول به سبد خرید اضافه شد");
//                }
//                else
//                {
//                    return BadRequest("این محصول در سبد خرید موجود است");
//                }
//            }
//        }
//        //////////

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPut("sendToPending")]
//        public async Task<IActionResult> SendToAdmin(int id, int? addressId)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var user = await _userRepo.TableNoTracking.Where(u=> u.IsVisible && u.ID == userid).Include(u=> u.CyProfile).FirstOrDefaultAsync();
//            if (user == null) return Unauthorized(new { msg = "کاربر پیدا نشد" });
//            var userName = user.CyProfile?.Name != null ? user.CyProfile.Name : "کاربر";
//            var Order = await _repo.Find(id);
//            CyAddress? Address = null;
//            if(addressId != null)
//                Address = await _addressRepo.Find((int)addressId);

//            //if (Address == null)
//            //    return BadRequest("آدرس معتبر نیست");

//            if (Order == null)
//                return NotFound("سفارش پیدا نشد");

//            if (Order.CyUserID != userid)
//                return Unauthorized("شما به این سفارش دسترسی ندارید");

//            if (Address != null && Address.CyUserID != userid)
//                return Unauthorized("شما به این آدرس دسترسی ندارید");

//            if (Order.Status != OrderStatus.InBasket)
//                return BadRequest("این سفارش در مرحله سبد خرید نیست");

//            if (Order.OrderMode != Ordermode.Normal)
//                return BadRequest("سفارش از نوع بازرسی یا پی سی بی است");

//            if(Address != null)
//                Order.CyAddressID = addressId;

//            Order.Status = OrderStatus.Pending;
//            Order.OrderDate = DateTime.Now;
//            Order.StatusText = "ارسال جهت استعلام";
//            Order.Notification = true;
//            //For Chipyab:
//            await _smsService.SendSmsAsync(user.CyUsNm, "8xsvjkl7jazov6h", userName, Order.ID);

//            return Ok(await _repo.Update(Order));
//        }

//        //[TypeAuthorize([UserType.SalesManager, UserType.Reporter])]
//        //[TypeAuthorize([UserType.SysAdmin])]
//        [HttpPost("GetOrdersByStatusAdmin")]
//        public async Task<IActionResult> GetOrdersByStatusAdmin(PagedOrdersDto Input)
//        {
//            var skipCount = Input.PageNumber * Input.PageSize;
//            int AllCount;
//            List<OrderReturnDTO> CyOrders;
//            if (Input.OrderStatus != OrderStatus.Draft)
//            {
//                var query = _repo.TableNoTracking.Where(o => o.Status == Input.OrderStatus && o.IsVisible && o.OrderMode == Ordermode.Normal);

//                AllCount = await query.CountAsync();

//                CyOrders = await query.OrderByDescending(o => o.CreateDate)
//                                                .Skip(skipCount).Take(Input.PageSize)
//                                                .Select(o => new OrderReturnDTO
//                                                {
//                                                    ID = o.ID,
//                                                    CyUserID = o.CyUserID,
//                                                    UserName = o.CyUser.CyProfile != null ? o.CyUser.CyProfile.Name : null,
//                                                    StatusText = o.StatusText,
//                                                    CyAddressID = o.CyAddressID,
//                                                    OrderDate = o.OrderDate != null ? o.OrderDate.Value : null,
//                                                    InProcessDate = o.InProcessDate,
//                                                    FinalizedDate = o.FinalizedDate,
//                                                    TotalAmount = o.TotalAmount != null ?  o.TotalAmount.Value : 0,
//                                                    Notification = o.Notification,
//                                                    CyOrderItems = new List<OrderItemReturnDTO>()
//                                                }).ToListAsync();

//            }
//            else
//            {
//                var query = _repo.TableNoTracking.Where(o => o.Status != OrderStatus.InBasket &&
//                                                                 o.Status != OrderStatus.InBOM &&
//                                                                 o.Status != OrderStatus.Draft &&
//                                                                 o.OrderMode == Ordermode.Normal &&
//                                                                 o.IsVisible);

//                AllCount = await query.CountAsync();

//                CyOrders = await query.Include(o=> o.CyUser).OrderByDescending(o => o.CreateDate)
//                                                                 .Skip(skipCount).Take(Input.PageSize)
//                                                                 .Select(o => new OrderReturnDTO
//                                                                 {
//                                                                     ID = o.ID,
//                                                                     CyUserID = o.CyUserID,
//                                                                     UserName = o.CyUser.CyProfile != null ? o.CyUser.CyProfile.Name : null,
//                                                                     StatusText = o.StatusText,
//                                                                     CyAddressID = o.CyAddressID,
//                                                                     OrderDate = o.OrderDate != null ? o.OrderDate.Value : null,
//                                                                     TotalAmount = o.TotalAmount != null ? o.TotalAmount.Value : 0,
//                                                                     Notification = o.Notification,
//                                                                     CyOrderItems = new List<OrderItemReturnDTO>()
//                                                                 }).ToListAsync();
//            }

//            var pagedRes = new PageResultDTO<OrderReturnDTO>
//            {
//                AllCount = AllCount,
//                ItemList = CyOrders,
//            };

//            return Ok(pagedRes);
//        }

//        [TypeAuthorize([UserType.SalesManager, UserType.Reporter])]
//        //[TypeAuthorize([UserType.SysAdmin])]
//        [HttpPost("GetOrderDetailsAdmin")]
//        public async Task<IActionResult> GetOrderDetailsAdmin(int OrderId)
//        {
//            var OrderProducts = await _itemRepo.Table.Where(i => i.IsVisible
//                                                      && i.CyOrderID == OrderId)
//                                               .Select(c => new OrderItemReturnDTO()
//                                               {
//                                                   ID = c.ID,
//                                                   CyProductID = c.CyProductID,
//                                                   Status = c.Status,
//                                                   StatusText= c.StatusText,
//                                                   Information = c.Information,
//                                                   CyProductImgUrl = c.CyProduct == null ? null : c.CyProduct.SmallImage,
//                                                   Quantity = c.Quantity,
//                                                   CyOrderID = c.CyOrderID,
//                                                   //UnitPrice = c.CyProduct == null ? null : c.CyProduct.Price,
//                                                   UnitPrice = c.UnitPrice,
//                                                   TotalPrice = c.TotalPrice,
//                                                   UnitOfferPrice = c.CyProduct == null ? null : c.CyProduct.Price,
//                                                   PartNumber = c.PartNumber,
//                                                   Manufacturer = string.IsNullOrEmpty(c.Manufacturer) ? null : c.Manufacturer,
//                                                   Duration = c.Duration,
//                                                   ProductCode = c.CyProduct == null ? null : c.CyProduct.ProductCode
//                                               }).ToListAsync();
//            return Ok(OrderProducts);
//        }

//        [TypeAuthorize([UserType.SalesManager, UserType.Reporter])]
//        [HttpGet("GetOrderItemById")]
//        public async Task<ActionResult<OrderItemReturnDTO>> getOrderItemById(int itemId)
//        {
//            var item = await _itemRepo.TableNoTracking.Where(i => i.IsVisible && i.ID == itemId)
//                                                               .Select(c => new OrderItemReturnDTO()
//                                                               {
//                                                                   ID = c.ID,
//                                                                   CyProductID = c.CyProductID,
//                                                                   Status = c.Status,
//                                                                   StatusText = c.StatusText,
//                                                                   Information = c.Information,
//                                                                   CyProductImgUrl = c.CyProduct == null ? null : c.CyProduct.SmallImage,
//                                                                   Quantity = c.Quantity,
//                                                                   CyOrderID = c.CyOrderID,
//                                                                   //UnitPrice = c.CyProduct == null ? null : c.CyProduct.Price,
//                                                                   UnitPrice = c.UnitPrice,
//                                                                   TotalPrice = c.TotalPrice,
//                                                                   UnitOfferPrice = c.CyProduct == null ? null : c.CyProduct.Price,
//                                                                   PartNumber = c.PartNumber,
//                                                                   Manufacturer = string.IsNullOrEmpty(c.Manufacturer) ? null : c.Manufacturer,
//                                                                   Duration = c.Duration,
//                                                                   ProductCode = c.CyProduct == null ? null : c.CyProduct.ProductCode
//                                                               }).FirstOrDefaultAsync();

//            if(item != null) return Ok(item);

//            return (NotFound());

//        }

//        [TypeAuthorize([UserType.SalesManager])]
//        //for admins 
//        [HttpPost("FinalizeOrder")]
//        public async Task<IActionResult> FinalizeOrder(int OrderId, [FromBody] ICollection<OrderItemVadidationDTO> items)
//        {
//            var myOrder = await _db.CyOrder.Where(c => c.ID == OrderId && c.IsVisible && c.Status == OrderStatus.Pending &&
//                                                  c.OrderMode == Ordermode.Normal)
//                                         .Include(c => c.CyOrderItems.Where(c => c.IsVisible))
//                                         .FirstOrDefaultAsync();
//            if (myOrder != null && myOrder.CyOrderItems != null)
//            {
//                var user = _userRepo.TableNoTracking.Where(u=> u.IsVisible && u.ID == myOrder.CyUserID).Include(c=>c.CyProfile).FirstOrDefault();
//                var OrderItems = myOrder.CyOrderItems.ToList();
//                if (user == null) return Unauthorized(new { msg = "کاربر پیدا نشد" });
//                var userName = user.CyProfile?.Name != null ? user.CyProfile.Name : "کاربر";

//                foreach (var item in items)
//                {
//                    var OldItem = OrderItems.Find(c => c.ID == item.ID);
//                    if (OldItem == null || !OldItem.IsVisible)
//                        return NotFound();

//                    //if (OldItem.CyOrderID != OrderId)
//                    //    return BadRequest("this id does not belong to this order");

//                    else
//                    {
//                        OldItem.Status = item.Status;
//                        OldItem.Quantity = item.Quantity;
//                        OldItem.UnitPrice = item.Status == OrderItemStatus.Active ? item.UnitPrice : null;
//                        OldItem.TotalPrice = item.Status == OrderItemStatus.Active ? (item.Quantity * item.UnitPrice) : null;
//                        OldItem.LastModified = DateTime.Now;
//                        OldItem.Duration = item.Duration;
//                    }
//                }

//                myOrder.Status = OrderStatus.Finalized;
//                myOrder.StatusText = "در انتظار تایید مشتری";
//                myOrder.TotalAmount = OrderItems.Where(c => c.Status == OrderItemStatus.Active).Sum(c => c.UnitPrice * c.Quantity);
//                myOrder.FinalizedDate = DateTime.Now;
//                myOrder.LastModified = DateTime.Now;
//                myOrder.Notification = false;
//                int i = await _db.SaveChangesAsync();
//                await _smsService.SendSmsAsync(user.CyUsNm, "mdaq86f9ktesb5r", userName, myOrder.ID);
//                return Ok("myOrder changed to Filalized with success " + i + " rocords affected");
//            }
//            else
//            {
//                return BadRequest("Order Not Valid!!");
//            }
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPost("ValidateOrder")]
//        public async Task<IActionResult> ValidateOrder(int OrderID, OrderStatus newStatus)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//             var Order = await _repo.Table.Where(o => o.IsVisible &&
//                                                o.ID == OrderID &&
//                                                (o.Status == OrderStatus.Finalized ||
//                                                 o.Status == OrderStatus.Draft ||
//                                                 o.Status == OrderStatus.Canceled) &&
//                                                o.OrderMode == Ordermode.Normal).Include(o => o.CyOrderItems.Where(i => i.IsVisible)).FirstOrDefaultAsync();
//            var user = await _userRepo.TableNoTracking.Where(u=> u.IsVisible && u.ID == userid).Include(c=> c.CyProfile).FirstOrDefaultAsync();
//            if (user == null) return Unauthorized(new { msg = "کاربر پیدا نشد" });
//            var userName = user.CyProfile?.Name != null ? user.CyProfile.Name : "کاربر";

//            if (Order == null)
//                return NotFound("سفارشی با این مشخصات پیدا نشد");

//            if (Order.CyUserID != userid)
//                return Unauthorized();

//            switch (newStatus)
//            {
//                case OrderStatus.Canceled:
//                    Order.Status = OrderStatus.Canceled;
//                    Order.CancelationDate = DateTime.Now;
//                    Order.LastModified = DateTime.Now;
//                    Order.StatusText = "لغو شده";
//                    break;

//                case OrderStatus.Verified:
//                    if(Order.Status == OrderStatus.Canceled || Order.Status == OrderStatus.Draft) return BadRequest("Invalid order status");
//                    Order.Status = OrderStatus.Verified;
//                    Order.VerifiedDate = DateTime.Now;
//                    Order.LastModified = DateTime.Now;
//                    Order.StatusText = "تایید مشتری";
//                    Order.Notification = true;
//                    await _smsService.SendSmsAsync(user.CyUsNm, "psr0m2u6fi31def", userName, Order.ID);
//                    break;
//                //????
//                case OrderStatus.InBasket:
//                    var oldBasket = _repo.Table.Where(o => o.IsVisible && o.Status == OrderStatus.InBasket && o.CyUserID == userid).FirstOrDefault();
//                    if (oldBasket != null)
//                    {
//                        oldBasket.Status = OrderStatus.Draft;
//                        oldBasket.StatusText = "در حالت پیشنویس";
//                        oldBasket.LastModified = DateTime.Now;
//                    }
//                    foreach(var item in Order.CyOrderItems)
//                    {
//                        item.Status = OrderItemStatus.Pending;
//                    }
//                    Order.Status = OrderStatus.InBasket;
//                    Order.StatusText = null;
//                    Order.LastModified = DateTime.Now;
//                    Order.OrderDate = DateTime.Now;
//                    Order.FinalizedDate = null;
//                    Order.InProcessDate = null;
//                    break;

//                default:
//                    return BadRequest("Invalid order status");
//            }
//            return Ok(await _db.SaveChangesAsync());
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPost("GetOrdersByStatus")]
//        public async Task<IActionResult> GetOrdersByStatus(OrderStatus Status)
//        {
//            if (Status == OrderStatus.InBasket)
//                return BadRequest("امکان دریافت این سفارش وجود ندارد");

//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var OrderQuery = _repo.Table.Where(o => o.IsVisible && o.CyUserID == userid && o.OrderMode == Ordermode.Normal);
//            if (Status == OrderStatus.Pending || Status == OrderStatus.Finalized || Status == OrderStatus.Verified)
//            {
//                OrderQuery = OrderQuery.Where(o => o.Status == OrderStatus.Finalized ||
//                                                   o.Status == OrderStatus.Pending ||
//                                                   o.Status == OrderStatus.Verified);
//            }
//            else if (Status == OrderStatus.Canceled)
//            {
//                OrderQuery = OrderQuery.Where(o => o.IsVisible && (o.Status == OrderStatus.Canceled || o.Status == OrderStatus.Draft));
//            }
//            else
//            {
//                OrderQuery = OrderQuery.Where(o => o.IsVisible && o.Status == Status);
//            }
//            var Orders = await OrderQuery.ToListAsync();
//            if (Orders == null)
//                return NotFound("شما سفارشی در این مرحله ندارید");
//            else
//                return Ok(_mapper.Map<ICollection<OrderDTO>>(Orders));
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPost("GetOrderDetails")]
//        public async Task<IActionResult> GetOrderDetails(int OrderId)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var myOrder = _repo.Table.Where(o => o.IsVisible && o.ID == OrderId).FirstOrDefault();
//            if (myOrder == null)
//            {
//                return NotFound("سفارش مورد نظر پیدا نشد");
//            }
//            if (myOrder.CyUserID != userid)
//                return Unauthorized("شما به این سفارش دسترسی ندارید");

//            var OrderProducts = await _itemRepo.Table.Where(i => i.IsVisible
//                                                      && i.CyOrderID == OrderId)
//                                               .Select(c => new OrderItemReturnDTO()
//                                               {
//                                                   ID = c.ID,
//                                                   CyProductID = c.CyProductID,
//                                                   Status = c.Status,
//                                                   CyProductImgUrl = c.CyProduct == null ? null : c.CyProduct.SmallImage,
//                                                   Quantity = c.Quantity,
//                                                   CyOrderID = c.CyOrderID,
//                                                   UnitPrice = c.UnitPrice,
//                                                   StatusText = c.StatusText,
//                                                   Information = c.Information,
//                                                   UnitOfferPrice = c.CyProduct == null ? null : c.UnitPrice,
//                                                   TotalPrice = c.Quantity * c.UnitPrice,
//                                                   PartNumber = c.PartNumber,
//                                                   Manufacturer = string.IsNullOrEmpty(c.Manufacturer) ? null : c.Manufacturer,
//                                                   Duration = c.Duration,
//                                                   ProductCode = c.CyProduct == null ? null : c.CyProduct.ProductCode,
//                                                   Supply = c.CyProduct == null ? 0 : c.CyProduct.Supply
//                                               }).ToListAsync();
//            return Ok(OrderProducts);
//        }

//        //[TypeAuthorize([UserType.Customer])]
//        //Unused
//        [AllowAnonymous]
//        [HttpPost("CreateOrderByFile")]
//        public async Task<IActionResult> CreateOrderByFile(Guid fileId)
//        {
//            int? userid = null;
//            var identity = User.Identities.FirstOrDefault();
//            if (identity !=null && identity.IsAuthenticated)
//                userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            
//            var currentBOM = await _db.CyOrder.Where(bom => bom.IsVisible && bom.Status == OrderStatus.InBOM && bom.CyUserID == userid).FirstOrDefaultAsync();
//            if (currentBOM != null)
//                return BadRequest(new { err = "شما یک سفارش BOM دارید" });

//            var selFile = await _db.CyFile.Where(c => c.FileIQ == fileId).FirstOrDefaultAsync();
//            if (selFile != null)
//            {
//                var BaseFolderPath = Path.Combine("FileContents", "private", selFile.FolderName);
//                var RelatedAddress = Path.Combine(BaseFolderPath, selFile.FileIQ + selFile.FileType);
//                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), RelatedAddress);

//                if (!System.IO.File.Exists(fullPath))
//                {
//                    return BadRequest(new { err = "File not Exist!!" });
//                }
//                var fileItems = _excelReader.ReadExcel(fullPath);
//                if(fileItems == null || fileItems.Count <= 0) return NoContent();
//                var insertedBOM = new CyOrder()
//                {
//                    CreateDate = DateTime.Now,
//                    IsVisible = true,
//                    Status = OrderStatus.InBOM,
//                    OrderMode = Ordermode.Normal,
//                    CyUserID = userid,
//                    BomFileID = fileId,
//                    CyOrderItems = new List<CyOrderItem>()
//                };

//                if (userid != null)
//                {
//                    insertedBOM = await _repo.Insert(insertedBOM); 
//                }

//                var logoutItems = new List<OrderItemReturnDTO>();
//                foreach (var item in fileItems)
//                {
//                    var selProduct = await _productRepo.TableNoTracking.Where(p =>  p.PartNo == item.PartNumber
//                                                                     && p.IsVisible).Select(o=> new SelectBomDTO()
//                                                                      {
//                                                                          ID = o.ID,
//                                                                          ManufacturerName = o.CyManufacturer != null ? o.CyManufacturer.Name : null,
//                                                                          Price = o.Price,
//                                                                          Supply = o.Supply
//                                                                      }).FirstOrDefaultAsync();
//                    var orderItem = new CyOrderItem()
//                    {
//                        IsVisible = true,
//                        CreateDate = DateTime.Now,
//                        CyOrderID = insertedBOM.ID,
//                        CyProductID = selProduct != null ? selProduct.ID : null,
//                        Quantity = item.Quantity,
//                        UnitPrice = selProduct != null ? selProduct.Price : null,
//                        TotalPrice = selProduct != null ? (item.Quantity * selProduct.Price) : null,
//                        PartNumber = item.PartNumber,
//                        Manufacturer = selProduct != null ? selProduct.ManufacturerName : item.Manufacturer
//                    };
//                    if (userid != null)
//                        insertedBOM.CyOrderItems.Add(orderItem);
//                    else
//                    {
//                        var orderItemReturn = _mapper.Map<OrderItemReturnDTO>(orderItem);
//                        orderItemReturn.Supply = selProduct != null ? selProduct.Supply : 0;
//                        logoutItems.Add(orderItemReturn);
//                    }
//                }
//                if (userid != null)
//                {
//                    await _db.SaveChangesAsync();
//                    return Ok(insertedBOM.ID);
//                }
//                else
//                    return Ok(logoutItems);
//            }
//            return BadRequest(new {err = "file not found"});
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPost("CreateOrderByFileV2")]
//        public async Task<IActionResult> CreateOrderByFileV2(Guid fileId)
//        {
//            int? userid = null;
//            var identity = User.Identities.FirstOrDefault();
//            if (identity != null && identity.IsAuthenticated)
//                userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
//            else return BadRequest(new {msg = "ابتدا وارد شوید",});

//            var selFile = await _db.CyFile.Where(c => c.FileIQ == fileId).FirstOrDefaultAsync();
//            var user = await _db.CyUser.Where(u => u.IsVisible && u.ID == userid).Include(u=> u.CyProfile).FirstOrDefaultAsync();
//            if (user == null || user.CyProfile == null) return BadRequest();
//            var username = user.CyProfile.Name != null ? user.CyProfile.Name : "مشتری";
//            if (selFile != null)
//            {
//                var insertedBOM = new CyOrder()
//                {
//                    CreateDate = DateTime.Now,
//                    OrderDate = DateTime.Now,
//                    IsVisible = true,
//                    Status = OrderStatus.Pending,
//                    OrderMode = Ordermode.Bom,
//                    CyUserID = userid,
//                    BomFileID = fileId,
//                    Notification = true,
//                    CyOrderItems = new List<CyOrderItem>()
//                };
//                var bom = await _repo.Insert(insertedBOM);
//                await _smsService.SendSmsAsync(user.CyUsNm, "8xsvjkl7jazov6h", username, bom.ID);
//                return Ok(bom);
//            }
//            return NotFound(new {msg = "فایل پیدا نشد"});
//        }

//        //[TypeAuthorize([UserType.Customer])]
//        [HttpGet("getUserBomOrders")]
//        public async Task<IActionResult> getBomOrders()
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null || !identity.IsAuthenticated)
//                return BadRequest(new { msg = "ابتدا وارد شوید"});
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var OrderQuery = _repo.Table.Where(o => o.IsVisible && o.CyUserID == userid && o.OrderMode == Ordermode.Bom);
            
//            var bomOrders = await _repo.TableNoTracking.Where(o=> o.IsVisible && o.CyUserID == userid && o.OrderMode == Ordermode.Bom).ToListAsync();
//            var mappedOrders = _mapper.Map<List<OrderDTO>>(bomOrders);
//            return Ok(mappedOrders);
//        }


//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("getUserBomOrdersV2")]
//        public async Task<IActionResult> getBomOrdersV2(OrderStatus status)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null || !identity.IsAuthenticated)
//                return BadRequest(new { msg = "ابتدا وارد شوید" });
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var OrderQuery = _repo.TableNoTracking.Where(o => o.IsVisible && o.CyUserID == userid && o.OrderMode == Ordermode.Bom);
//            if (OrderStatus.InProcess == status)
//            {
//                OrderQuery = OrderQuery.Where(o => o.Status == OrderStatus.InProcess || o.Status == OrderStatus.Verified || o.Status == OrderStatus.IsSent);
//            }
//            else if(status == OrderStatus.Pending)
//            {
//                OrderQuery = OrderQuery.Where(o=> o.Status == OrderStatus.Pending || o.Status == OrderStatus.Finalized);
//            }
//            else if(status == OrderStatus.Delivered)
//            {
//                OrderQuery = OrderQuery.Where(o => o.Status == OrderStatus.Delivered);
//            }
//            else if(status == OrderStatus.Canceled)
//            {
//                OrderQuery = OrderQuery.Where(o => o.Status == OrderStatus.Canceled);
//            }
//            var bomOrders = await OrderQuery.ToListAsync();
//            var mappedOrders = _mapper.Map<List<OrderDTO>>(bomOrders);
//            //return Ok(new { Orders = mappedOrders, totalPrice = mappedOrders.Select(x => x.) });
//            return Ok(mappedOrders);
//        }

//        ////for admin
//        //[TypeAuthorize([UserType.Customer])]
//        //[HttpPost("createO")]

//        //for admin
//        //[TypeAuthorize([UserType.SalesManager, UserType.Reporter])]
//        [HttpGet("getAllBomOrdersV2")]
//        public async Task<IActionResult> getAllBomOrders()
//        {
//            var result = await _repo.TableNoTracking.Where(o=> o.IsVisible && o.OrderMode == Ordermode.Bom).Select(b=> new OrderDTO()
//            {
//                BomFileID = b.BomFileID,
//                CyAddressID = b.CyAddressID,
//                OrderDate = b.OrderDate != null ? b.OrderDate.Value : b.CreateDate,
//                InProccessDate = b.InProcessDate,
//                FinalizedDate = b.FinalizedDate,
//                Name = (b.CyUser != null && b.CyUser.CyProfile != null) ? b.CyUser.CyProfile.Name : null,
//                Status = b.Status,
//                StatusText = b.StatusText,
//                ID = b.ID,
//                CyUserID = b.CyUserID,
//                TotalAmount = b.TotalAmount != null ? b.TotalAmount.Value : 0,
//                Notification = b.Notification,
//            }).ToListAsync();
//            //var mappedResult = _mapper.Map<List<OrderDTO>>(result);
//            return Ok(result);
//        }

//        //for admin
//        [TypeAuthorize([UserType.SalesManager])]
//        [HttpPost("uploadFinalExcelV2")]
//        public async Task<ActionResult<List<CyOrderItem>>> postOrderItemExcel(UploadExcelDTO inputs)
//        {
//            var order = await _repo.Table.Where(o=> o.IsVisible && o.ID == inputs.orderId).Include(o=> o.CyOrderItems).FirstOrDefaultAsync();
//            if(order == null) return NotFound(new { msg = "سفارش پیدا نشد" });

//            var selFile = await _db.CyFile.Where(c => c.FileIQ == inputs.fileId).FirstOrDefaultAsync();
//            if (selFile != null)
//            {
//                var BaseFolderPath = Path.Combine("FileContents", "private", selFile.FolderName);
//                var RelatedAddress = Path.Combine(BaseFolderPath, selFile.FileIQ + selFile.FileType);
//                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), RelatedAddress);

//                if (!System.IO.File.Exists(fullPath))
//                {
//                    return BadRequest(new { err = "File not Exist!!" });
//                }
//                var fileItems = _excelReader.ReadExcelV2(fullPath);
//                if (fileItems == null || fileItems.Count <= 0) return BadRequest(new { msg = "فایل خالی است" });

               
//                order.CyOrderItems = new List<CyOrderItem>();
//                order.CyOrderItems.Clear();
//                foreach (var item in fileItems)
//                {
//                    order.CyOrderItems.Add(new CyOrderItem()
//                    {
//                        IsVisible = true,
//                        CreateDate = DateTime.Now,
//                        CyOrderID = order.ID,
//                        Information = item.Information,
//                        Manufacturer = item.Manufacturer,
//                        Status = OrderItemStatus.Pending,
//                        PartNumber = item.PartNumber,
//                        Duration = item.Duration,
//                        TotalPrice = item.TotalPrice,
//                        UnitPrice = item.UnitPrice,
//                        Quantity = item.Quantity,
//                        StatusText = item.StatusText,
//                    });
//                }
//                order.FinalizedDate = DateTime.Now;
//                order.Notification = false;
//                await _db.SaveChangesAsync();
//                return Ok(fileItems);
//            }
//            return NotFound(new { msg = "فایل پیدا نشد" });
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPost("addItemToBOM")]
//        public async Task<IActionResult> AddItemToBOM([FromBody] PartInfo newItem)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var BOMorder = await _db.CyOrder.Where(o => o.Status == OrderStatus.InBOM && o.IsVisible && o.CyUserID == userid).FirstOrDefaultAsync();

//            if (BOMorder == null)
//            {
//                BOMorder = new CyOrder()
//                {
//                    CreateDate = DateTime.Now,
//                    Status = OrderStatus.InBOM,
//                    CyUserID = userid,
//                    CyOrderItems = new List<CyOrderItem>(),
//                    IsVisible = true,
//                };
//                BOMorder = await _repo.Insert(BOMorder);
//            }
            
//            var selProduct = await _db.CyProduct.Where(p => ( p.ProductCode == newItem.PartNumber ||
//                                                              p.PartNo == newItem.PartNumber ||
//                                                              p.Name == newItem.PartNumber) && p.IsVisible).FirstOrDefaultAsync();
//            var newOrderItem = new CyOrderItem()
//            {
//                IsVisible = true,
//                CreateDate = DateTime.Now,
//                CyOrderID = BOMorder.ID,
//                CyProductID = selProduct?.ID,
//                Quantity = newItem.Quantity,
//                UnitPrice = selProduct?.Price,
//                TotalPrice = selProduct != null ? (newItem.Quantity * selProduct.Price) : null,
//                PartNumber = newItem.PartNumber,
//                Manufacturer = newItem?.Manufacturer
//            };
//            await _itemRepo.Insert(newOrderItem);
//            return Ok();
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPut("updateBomItem")]
//        public async Task<ActionResult> updateBomItem([FromBody] BomItemUpdateDto form)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var item = _itemRepo.Table.Where(i=> i.IsVisible && i.ID == form.itemId && i.CyOrder != null && i.CyOrder.CyUserID == userid).FirstOrDefault();
//            if(item == null)
//                return NotFound("آیتم مورد نظر پیدا نشد");
//            item.Quantity = form.quantity;
//            _db.Update(item);
//            await _db.SaveChangesAsync();
//            return Ok();
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPut("createBomOnLogin")]
//        public async Task<IActionResult> CreateBomOnLogin([FromBody] List<BomPartInfoDTO> parts, Guid fileId)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var pervBasket = await _repo.Table.Where(o => o.IsVisible && o.Status == OrderStatus.InBasket && o.CyUserID == userid).FirstOrDefaultAsync();
//            if (pervBasket != null)
//                pervBasket.IsVisible = false;

//            var BOMorderToBasket = new CyOrder()
//                    {
//                    CreateDate = DateTime.Now,
//                    IsVisible = true,
//                    OrderDate = DateTime.Now,
//                    Status = OrderStatus.InBasket,
//                    OrderMode = Ordermode.Normal,
//                    CyUserID = userid,
//                    BomFileID = fileId,
//                    CyOrderItems = new List<CyOrderItem>()
//                   };
//            BOMorderToBasket = await _repo.Insert(BOMorderToBasket);

//            foreach (var item in parts)
//            {
//                //var selProduct = await _productRepo.TableNoTracking.Where(p => p.PartNo == item.PartNumber
//                //                                                          && p.IsVisible).Select(o => new SelectBomDTO()
//                //                                                          {
//                //                                                              ID = o.ID,
//                //                                                              Price = o.Price,
//                //                                                              Supply = o.Supply
//                //                                                          }).FirstOrDefaultAsync();
//                BOMorderToBasket.CyOrderItems.Add(new CyOrderItem()
//                {
//                    IsVisible = true,
//                    CreateDate = DateTime.Now,
//                    CyOrderID = BOMorderToBasket.ID,
//                    CyProductID = item.CyProductID, //selProduct != null ? selProduct.ID : null,
//                    Quantity = item.Quantity,
//                    UnitPrice = item.UnitPrice,//selProduct != null ? selProduct.Price : null,
//                    TotalPrice = item.TotalPrice,//selProduct != null ? (item.Quantity * selProduct.Price) : null,
//                    PartNumber = item.PartNumber,
//                    Manufacturer = item.Manufacturer //selProduct != null ? null : item.Manufacturer
//                });
//            }

//            return Ok(await _db.SaveChangesAsync());
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPut("moveBOMtoBasket")]
//        public async Task<IActionResult> MoveBOMtoBasket()
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var selOrder = await _db.CyOrder.Where(o => o.IsVisible && o.Status == OrderStatus.InBOM && o.CyUserID == userid).FirstOrDefaultAsync();
//            if (selOrder == null)
//                return BadRequest("شما سفارشی ندارید");

//            selOrder.Status = OrderStatus.InBasket;

//            var selBasket = await _db.CyOrder.Where(o => o.CyUserID == userid &&
//                                                    o.IsVisible &&
//                                                    o.Status == OrderStatus.InBasket)
//                                             .FirstOrDefaultAsync();
//            if (selBasket != null)
//                selBasket.IsVisible = false;

//            return Ok(await _db.SaveChangesAsync());
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpDelete("deleteBOM")]
//        public async Task<IActionResult> DeleteBOM(int Id)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var BomOrder = await _repo.Find(Id);
//                if (BomOrder != null && BomOrder.IsVisible &&
//                    BomOrder.Status == OrderStatus.InBOM &&
//               BomOrder.CyUserID == userid)
//                {
//                    return Ok(await _repo.Delete(Id));
//                }
//                else
//                {
//                    return BadRequest("مشکلی پیش آمد");
//                }
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPost("postInspection")]
//        public async Task<ActionResult<List<CyInspectionForm>>> postInspection([FromBody] InspectionFormDTO input)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
//            var user = await _db.CyUser.Where(u => u.ID == userid && u.IsVisible).Include(u => u.CyProfile).FirstOrDefaultAsync();
//            if (user == null || user.CyProfile == null) return BadRequest();    
//            //if (input.Items == null || input.Items.Count < 1)
//            //    return BadRequest();

//            var newInspection = _mapper.Map<CyInspectionForm>(input);

//            newInspection.UserID = userid;

//            if (newInspection.Lab == InspectionLab.Iran)
//            {
//                newInspection.PinCorrelationTest = 0;
//                newInspection.XRFTest = 0;
//                newInspection.KeyFunctional = 0;
//                newInspection.Baking = 0;
//                newInspection.TapeAndReel = 0;
//                newInspection.InternalVisualInspection = 0;
//            }
//            var insertedForm = await _inspectionRepo.Insert(newInspection);

//            var CyOrder = new CyOrder()
//            {
//                CreateDate = DateTime.Now,
//                OrderDate = DateTime.Now,
//                OrderMode = Ordermode.Inspection,
//                CyInspectionFormID = insertedForm.ID,
//                CyUserID = userid,
//                CyAddressID = input.CyAddressID,
//                IsVisible = true,
//                Status = OrderStatus.Pending,
//                Notification = true,
//                StatusText = "ارسال جهت بررسی"
//            };
//            CyOrder = await _repo.Insert(CyOrder);
//            await _repo.SaveChangesAsync();
//            //await _smsService.SendSmsAsync(user.CyUsNm, "8xsvjkl7jazov6h", userName, Order.ID);
//            await _smsService.SendSmsAsync(user.CyUsNm, "8xsvjkl7jazov6h", user.CyProfile.Name, CyOrder.ID);

//            return Ok("با موفقیت ثبت شد");
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("getUserInspectionByID")]
//        public async Task<ActionResult<List<InspectionOrderReturnDTO>>> getUserInspectionById(int InspectionOrderId)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var result = await _repo.TableNoTracking.Where(o => o.CyUserID == userid &&
//                                               o.IsVisible &&
//                                               o.OrderMode == Ordermode.Inspection &&
//                                               o.ID == InspectionOrderId)
//                                                .Include(o => o.CyInspectionForm).Include(o=> o.CyInspectionForm.Items)
//                                    .FirstOrDefaultAsync();
//            return Ok(_mapper.Map<InspectionOrderReturnDTO>(result));
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("getUserInspections")]
//        public async Task<ActionResult<List<InspectionOrderReturnDTO>>> getUserInspections()
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var result = await _repo.TableNoTracking.Where(o => o.CyUserID == userid &&
//                                               o.IsVisible &&
//                                               o.OrderMode == Ordermode.Inspection)
//                                    .ToListAsync();
//            return Ok(_mapper.Map<List<InspectionOrderReturnDTO>>(result));
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("getUserInspectionsV2")]
//        public async Task<ActionResult<List<InspectionOrderReturnDTO>>> getUserInspectionsV2(OrderStatus status)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
//            var OrderQuery = _repo.TableNoTracking.Where(o => o.CyUserID == userid &&
//                                               o.IsVisible &&
//                                               o.OrderMode == Ordermode.Inspection);
//            if(status == OrderStatus.Pending)
//            {
//                OrderQuery = OrderQuery.Where(o=> o.Status == OrderStatus.Finalized || o.Status == OrderStatus.Pending);
//            }
//            else if(status == OrderStatus.InProcess)
//            {
//                OrderQuery = OrderQuery.Where(o => o.Status == OrderStatus.InProcess || o.Status == OrderStatus.Verified || o.Status == OrderStatus.IsSent);
//            }
//            else if(status == OrderStatus.Delivered)
//            {
//                OrderQuery = OrderQuery.Where(o=> o.Status == OrderStatus.Delivered);
//            }
//            else if(status == OrderStatus.Canceled)
//            {
//                OrderQuery = OrderQuery.Where(o => o.Status == OrderStatus.Canceled);
//            }
//            var result = await OrderQuery
//                                    .ToListAsync();
//            return Ok(_mapper.Map<List<InspectionOrderReturnDTO>>(result));
//        }


//        //[TypeAuthorize(UserType.SysAdmin)]
//        //[TypeAuthorize([UserType.SalesManager, UserType.Reporter])]
//        [HttpPost("getAllInspectionOrders")]
//        public async Task<ActionResult<PageResultDTO<InspectionOrderReturnDTO>>> getAllInspections(PagedProdCatDTO input)
//        {
//            //var identity = User.Identities.FirstOrDefault();
//            //if (identity == null)
//            //    return Unauthorized("کاربر پیدا نشد");

//            var skipCount = input.PageNumber * input.PageSize;

//            var AllCount = await _repo.TableNoTracking.Where(o =>
//                                      o.IsVisible && o.OrderMode == Ordermode.Inspection)
//                                .CountAsync();
//            var result = await _repo.TableNoTracking.Where(o =>
//                                               o.IsVisible &&
//                                               o.OrderMode == Ordermode.Inspection)
//                                                .Select(p=> new InspectionOrderReturnDTO()
//                                                {
//                                                    InspectionForm = _mapper.Map<InspectionFormDTO>(p.CyInspectionForm),
//                                                    Name = p.CyUser != null ? p.CyUser.CyProfile.Name : null,
//                                                    CyAddressID = p.CyAddressID,
//                                                    CyUserID = p.CyUserID,
//                                                    OrderDate = p.OrderDate != null ? p.OrderDate.Value : p.CreateDate,
//                                                    ID = p.ID,
//                                                    Status = p.Status,
//                                                    Notification = p.Notification,
//                                                    StatusText = p.StatusText
//                                                })
//                                                //.Include(o => o.CyInspectionForm).Include(o => o.CyInspectionForm.Items)
//                                    .ToListAsync();

//            var pagedRes = new PageResultDTO<InspectionOrderReturnDTO>()
//            {
//                ItemList = result, //_mapper.Map<List<InspectionOrderReturnDTO>>(result),
//                AllCount = AllCount
//            };
//            return Ok(pagedRes);
//        }

//        //for Admin
//        [TypeAuthorize([UserType.SalesManager])]
//        [HttpPut("sendToInProccess")]
//        public async Task<IActionResult> sendToInProccessDelivered(int orderId, OrderStatus status)
//        {
//            var order = await _repo.Table.Where(o=> o.IsVisible && o.ID == orderId).FirstOrDefaultAsync();
//            if(order == null)
//                return BadRequest("سفارش معتبر نمی باشد");
//            var user = _userRepo.TableNoTracking.Where(u => u.IsVisible && u.ID == order.CyUserID).Include(c => c.CyProfile).FirstOrDefault();
//            if (user == null) return Unauthorized(new { msg = "کاربر پیدا نشد" });
//            var userName = user.CyProfile?.Name != null ? user.CyProfile.Name : "کاربر";

//            if (status == OrderStatus.InProcess && order.Status == OrderStatus.Verified)
//            {
//                order.Status = OrderStatus.InProcess;
//                order.LastModified = DateTime.Now;
//                order.InProcessDate = DateTime.Now;
//                order.Notification = false;
//                order.StatusText = "درحال تامین";
//            }
//            else if (status == OrderStatus.Delivered && order.Status == OrderStatus.InProcess)
//            {
//                order.Status = OrderStatus.Delivered;
//                order.LastModified = DateTime.Now;
//                order.Notification = false;
//                order.StatusText = "تحویل داده شده";
//                await _smsService.SendSmsAsync(user.CyUsNm, "juqwom1pfbfq8xh", userName, order.ID);
//            }
//            else
//            {
//                return BadRequest("سفارش معتبر نمی باشد");
//            }
//            return Ok(await _db.SaveChangesAsync());     
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpPost("createPcbFrom")]
//        public async Task<ActionResult<int>> createPCBform([FromBody] pcbDTO newPcb, int? AddressId)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var user = await _db.CyUser.Where(u => u.ID == userid && u.IsVisible).Include(u=>u.CyProfile).FirstOrDefaultAsync();
//            if (user == null || user.CyProfile == null) return NotFound();
//            var toInsertPcb = _mapper.Map<CyPcbForm>(newPcb);
//            toInsertPcb.UserID = userid;

//            toInsertPcb = await _pcbFormRepo.Insert(toInsertPcb);

//            var newOrder = new CyOrder() {
//                Status = OrderStatus.Pending,
//                OrderDate = DateTime.Now,
//                StatusText = "در انتظار استعلام",
//                CyPcbForm = toInsertPcb,
//                OrderMode = Ordermode.Pcb,
//                Notification = true,
//                CyUserID = userid,
//                CyAddressID = AddressId
//            };

//            newOrder = await _repo.Insert(newOrder);
//            await _smsService.SendSmsAsync(user.CyUsNm, "43m4g7uelvjg8jk", user.CyProfile.Name, newOrder.ID);
//            return Ok("سفارش با موفقیت ثبت شد");
//        }
     

//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("getPcbFormsForUser")]
//        public async Task<ActionResult<ICollection<pcbDTO>>> getUserPCBorders()
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var userPcbs = await _repo.TableNoTracking.Where(o=> o.IsVisible &&
//                                                             o.OrderMode == Ordermode.Pcb &&
//                                                             o.CyUserID == userid
//                                                             )
//                                                             //o.CyUserID == userid).Include(o=> o.CyPcbForm)
//                .Select(
//                o =>
//                new PcbOrderReturnDTO()
//                {
//                    ID = o.ID,
//                    CyUserID = userid,
//                    CyAddressID = o.CyAddressID,
//                    //PcbForm = _mapper.Map<pcbDTO>(o.CyPcbForm),
//                    StatusText = o.StatusText,
//                    Status = o.Status,
//                    OrderDate = o.CreateDate,
//                }
//                )
//                .ToListAsync();
//            return Ok(userPcbs);
//        }

//        [TypeAuthorize([UserType.Customer])]
//         [HttpGet("getPcbFormsForUserV2")]
//        public async Task<ActionResult<ICollection<pcbDTO>>> getUserPCBordersV2(OrderStatus Status)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
//            var OrderQuery = _repo.TableNoTracking.Where(o => o.IsVisible && o.OrderMode == Ordermode.Pcb && o.CyUserID == userid);
//            if(Status == OrderStatus.Pending)
//            {
//                OrderQuery = OrderQuery.Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Finalized);
//            }
//            else if(Status == OrderStatus.InProcess)
//            {
//                OrderQuery = OrderQuery.Where(o => o.Status == OrderStatus.InProcess || o.Status == OrderStatus.Verified || o.Status == OrderStatus.IsSent);
//            }
//            else if(Status == OrderStatus.Delivered)
//            {
//                OrderQuery = OrderQuery.Where(o => o.Status == OrderStatus.Delivered);
//            }
//            else if(Status == OrderStatus.Canceled)
//            {
//                OrderQuery = OrderQuery.Where(o => o.Status == OrderStatus.Canceled);
//            }

//            var userPcbs = await OrderQuery
//                .Select(
//                o =>
//                new PcbOrderReturnDTO()
//                {
//                    ID = o.ID,
//                    CyUserID = userid,
//                    CyAddressID = o.CyAddressID,
//                    StatusText = o.StatusText,
//                    Status = o.Status,
//                    OrderDate = o.CreateDate,
//                }
//                )
//                .ToListAsync();
//            return Ok(userPcbs);
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("getPcbFormByID")]
//        public async Task<ActionResult<ICollection<pcbDTO>>> getUserPCBorders(int PcbOrderId)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var userPcbs = await _repo.TableNoTracking.Where(o => o.IsVisible &&
//                                                             o.OrderMode == Ordermode.Pcb &&
//                                                             o.ID == PcbOrderId &&
//                o.CyUserID == userid).Include(o=> o.CyPcbForm)
//                .Select(
//                o =>
//                new PcbOrderReturnDTO()
//                {
//                    ID = o.ID,
//                    CyUserID = userid,
//                    CyAddressID = o.CyAddressID,
//                    PcbForm = _mapper.Map<pcbDTO>(o.CyPcbForm),
//                    StatusText = o.StatusText,
//                    OrderDate = o.CreateDate,
//                }
//                )
//                .ToListAsync();
//            return Ok(userPcbs);
//        }

//        //for admin
//        //[TypeAuthorize([UserType.SalesManager, UserType.Reporter])]
//        [HttpPost("getAllPcbForms")]
//        public async Task<ActionResult<PageResultDTO<PcbOrderReturnDTO>>> getAllPcbOrders(PageDTO pageInfo)
//        {
//            var skipCount = pageInfo.PageSize * pageInfo.PageNumber;

//            var AllPcbs = await _repo.TableNoTracking.Where(o => o.IsVisible &&
//                                                             o.OrderMode == Ordermode.Pcb
//                                                             ).Include(o => o.CyPcbForm)
//                                                              .Skip(skipCount).Take(pageInfo.PageSize)
//                                                              .Select(
//                                                              o =>
//                                                              new PcbOrderReturnDTO()
//                                                              {
//                                                                  ID = o.ID,
//                                                                  Name = o.CyUser != null ? o.CyUser.CyProfile != null ? o.CyUser.CyProfile.Name : null : null,
//                                                                  CyUserID = o.CyUserID,
//                                                                  CyAddressID = o.CyAddressID,
//                                                                  PcbForm = _mapper.Map<pcbDTO>(o.CyPcbForm),
//                                                                  Status = o.Status,
//                                                                  StatusText = o.StatusText,
//                                                                  OrderDate = o.CreateDate,
//                                                                  Notification = o.Notification
//                                                              })
//                                                             .ToListAsync();
//            var allCount = await _repo.TableNoTracking.Where(o => o.IsVisible && o.OrderMode == Ordermode.Pcb).CountAsync();
//            return Ok(new PageResultDTO<PcbOrderReturnDTO>()
//            {
//                ItemList = AllPcbs,
//                AllCount = allCount
//            });
//        }

//        //for admin
//        [TypeAuthorize([UserType.SalesManager])]
//        [HttpPut("updatePcbOrderStatus")]
//        public async Task<ActionResult> updatePcbForm([FromBody] UpdatePcbDTO inputs)
//        {
//            var pcbOrder = await _repo.Table.Where(o=> o.IsVisible && o.ID == inputs.OrderId && ( o.OrderMode == Ordermode.Pcb ||
//                                                                                                  o.OrderMode == Ordermode.Inspection ||
//                                                                                                  o.OrderMode == Ordermode.Bom)).Include(i=> i.CyOrderItems.Where(i=> i.IsVisible)).FirstOrDefaultAsync();
//            if (pcbOrder == null)
//                return NotFound("سفارش با این مشخصات پیدا نشد");
//            else
//            {
//                pcbOrder.Status = inputs.Status;
//                pcbOrder.StatusText = inputs.StatusText;
//                pcbOrder.Notification = false;
//                if (inputs.Status == OrderStatus.InProcess) pcbOrder.InProcessDate = DateTime.Now;
//                if(pcbOrder.OrderMode == Ordermode.Bom)
//                {
//                    foreach(var item in pcbOrder.CyOrderItems)
//                    {
//                        item.Status = OrderItemStatus.Active;
//                    }
//                    await _db.SaveChangesAsync();
//                }
//                await _repo.Update(pcbOrder);
//                return Ok("با موفقیت به روز رسانی شد");
//            }
//        }


//        //for admin
//        [TypeAuthorize([UserType.SalesManager])]
//        [HttpPut("updateItemStatus")]
//        public async Task<ActionResult> updateItemStatus([FromBody] UpdateItemStatusDTO inputs)
//        {
//            var selectedItem = _itemRepo.Table.Where(i=> i.IsVisible &&
//                                                     i.ID == inputs.ItemID).FirstOrDefault();
//            if (selectedItem == null) return NotFound("سفارش مورد نظر پیدا نشد");

//            selectedItem.StatusText = inputs.StatusText;
//            await _itemRepo.Update(selectedItem);
//            return Ok(selectedItem);
//        }


//        //for admin
//        [TypeAuthorize([UserType.SalesManager])]
//        [HttpPut("updateItemInfo")]
//        public async Task<ActionResult> updateItemInfo([FromBody] UpdateItemStatusDTO inputs)
//        {
//            var selectedItem = _itemRepo.Table.Where(i => i.IsVisible &&
//                                                     i.ID == inputs.ItemID).FirstOrDefault();
//            if (selectedItem == null) return NotFound("سفارش مورد نظر پیدا نشد");

//            selectedItem.Information = inputs.StatusText;
//            await _itemRepo.Update(selectedItem);
//            return Ok(selectedItem);
//        }
//    }
//}