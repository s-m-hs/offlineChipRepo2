using System.Security.Claims;
using AutoMapper;
using CY_BM;
using CY_DM;
using CY_WebApi.Migrations;
using CY_WebApi.Models;
using CY_WebApi.Services;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Google.Cloud.RecaptchaEnterprise.V1;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using static Google.Cloud.RecaptchaEnterprise.V1.TransactionData.Types;

namespace CY_WebApi.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class CyOrdersBController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CyContext _db;

        public CyOrdersBController(CyContext context, IMapper mapper)
        {
            _mapper = mapper;
            _db = context;
        }

        /// <summary>
        /// افزودن لیست سفارش جدید
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("addOrder")]
        public async Task<ActionResult> addOrder([FromBody] OrderDTO dto)
        {
            var userCliam = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userCliam, out Guid userId)) return Unauthorized(new { msg = "احراز هویت انجام نشده است ..." });
            if (userClaimRole == null) return BadRequest(new { msg = "نقش کاربر مشخص نیست" });

            UserType userType = UserTypeExtensionsB.GetRoleType(userClaimRole);

            List<UserType> userTypeS = new(){
                UserType.GroupExpert,UserType.GroupManager ,UserType.DepartmentManager,UserType.GeneralManager
            };


            var currentUser = await _db.CyUser
       .AnyAsync(x => x.IsVisible && x.ID == userId);
            if (currentUser == null) return BadRequest(new { msg = "کاربر یافت نشد" });


            if (dto.CyOrderItems == null || !dto.CyOrderItems.Any())
                return BadRequest(new { msg = "محصولی برای سفارش افزوده نشده است" });
            //؟؟؟
            var order = _mapper.Map<CyOrder>(dto);
            order.ID = Guid.NewGuid();
            order.CyUserID = userId;
            order.OrderDate = DateTime.Now;

            foreach (var item in order.CyOrderItems)
            {

                item.ID = Guid.NewGuid();
                item.CyOrderID = order.ID;

                foreach (var item1 in userTypeS)
                {
                    item.Approvals.Add(new OrderItemApproval
                    {
                        CyOrderItemID = item.ID,
                        Role = item1,
                        RoleName = item1.GetRoleName(),
                        IsApproved = item1 == userType ? true : false,
                        IsFirst = true,
                    });
                    item.Approvals.Add(new OrderItemApproval
                    {
                        CyOrderItemID = item.ID,
                        Role = item1,
                        RoleName = item1.GetRoleName(),
                        IsApproved = false,
                        IsFirst = false,
                    });
                }


            }
            //var orderUsers=await _db.CyUser.
            //    Where(x=>(dto.CyGoroohId !=null && x.CyGoroohID == dto.CyGoroohId) || (dto.CyVahedId != null && x.CyVahedID== dto.CyVahedId)
            //    || x.userType== UserType.PurchasingExpert || x.userType == UserType.PurchasingExpert).ToListAsync();

            //foreach (var user in orderUsers) {
            //   await _db.CyUserOrders.AddAsync(new CyUserOrder
            //    {
            //        UserID = user.ID,
            //        OrderID = order.ID,
            //    });
            //}



            var newDto = _mapper.Map<OrderDTO>(order);

            await _db.CyOrder.AddAsync(order);

            await _db.SaveChangesAsync();

            return Ok(newDto);
        }





        /// <summary>
        /// افزودن تک آیتم سفارش جدید
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("addOrderItemToOrder")]
        public async Task<ActionResult> addOrderItemToOrder([FromBody] OrderItemDTO dto)
        {
            List<UserType> userTypeS = new(){
                UserType.GroupExpert,UserType.GroupManager ,UserType.DepartmentManager,UserType.GeneralManager
            };
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if ((!Guid.TryParse(userClaim, out Guid userId)) || userClaimRole == null) return BadRequest(new { msg = "UserType Not Found" });

            var currentOrder = await _db.CyOrder.Where(x => x.IsVisible && x.ID == dto.CyOrderID).Include(i => i.CyOrderItems).FirstOrDefaultAsync();
            if (currentOrder == null) return BadRequest(new { msg = "Order Not Found" });
            if (currentOrder.Status == OrderStatus.Inquiring) return BadRequest(new { msg = "دراین مرحله مجاز به اعمال تغییر نیستید" });

            var currentOrderItem = _mapper.Map<CyOrderItem>(dto);
            currentOrderItem.CreatorId = userId;

            var userType = UserTypeExtensionsB.GetRoleType(userClaimRole);
            foreach (var item1 in userTypeS)
            {
                currentOrderItem.Approvals.Add(new OrderItemApproval
                {
                    CyOrderItemID = currentOrderItem.ID,
                    Role = item1,
                    RoleName = UserTypeExtensions.GetRoleName(item1),
                    IsApproved = item1 == userType ? true : false,
                    IsFirst = true,

                });
                currentOrderItem.Approvals.Add(new OrderItemApproval
                {
                    CyOrderItemID = currentOrderItem.ID,
                    Role = item1,
                    RoleName = UserTypeExtensions.GetRoleName(item1),
                    IsApproved = false,
                    IsFirst = false,

                });

            }


            currentOrder.CyOrderItems.Add(currentOrderItem);

            await _db.SaveChangesAsync();

            return Ok(currentOrderItem);


        }



        /// <summary>
        /// صرفا جهت تست
        /// </summary>
        /// <returns></returns>
        [HttpGet("getOrders")]
        public async Task<ActionResult> getOrders()
        {
            var orders = _db.CyOrder.Where(x => x.IsVisible).ToList();

            return Ok(orders);
        }

        /// <summary>
        /// گرفتن لیست سفارشهای کاربر 
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserOrder")]
        public async Task<ActionResult<OrderSelect>> UserOrder()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimB = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });

            var currentUser = _db.CyUser.Where(x => x.IsVisible && x.ID == userId).FirstOrDefault();
            if (currentUser == null) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });
            List<CyOrder> orderList = new();

            var groupId = currentUser.CyGoroohID;
            var vahedId = currentUser.CyVahedID;

            var ordersQuery = _db.CyOrder
    .Where(x => x.IsVisible)
    .Include(i => i.CyVahed)
    .Include(i => i.CyGorooh)
    .Include(i => i.CyUser)
    .AsQueryable();

            if (currentUser.userType == UserType.GroupExpert)
            {
                ordersQuery = ordersQuery.Where(x => x.CyUserID == userId);
            }
            else if (currentUser.userType == UserType.GroupManager)
            {
                ordersQuery = ordersQuery.Where(x => x.CyUserID == userId || x.CyGoroohId == groupId);
            }
            else if (currentUser.userType == UserType.DepartmentManager)
            {
                ordersQuery = ordersQuery.Where(x => x.CyUserID == userId || x.CyVahedId == vahedId);
            }
            else if (currentUser.userType == UserType.PurchasingExpert || currentUser.userType == UserType.PurchasingManager)
            {
                ordersQuery = ordersQuery.Where(x => x.Status != OrderStatus.Pending && x.Status != OrderStatus.AwaitingCustomerApproval);
            }


            // مدیر کل و بقیه → بدون فیلتر

            var result = await ordersQuery.Select(s => new OrderSelect
            {
                id = s.ID,
                orderCode = s.OrderCode,
                cyVahed = s.CyVahed.Text,
                cyGorooh = s.CyGorooh.Text,
                createDate = s.CreateDate,
                cyUser = s.CyUser.CyUsNm,
                totalAmount = s.TotalAmount,
                orderStatus = s.Status,
                startOrderDate = s.LastModified, ///زمان آغاز سفارش
                curency=s.Curency
            }).OrderByDescending(o => o.createDate).ToListAsync();

            return Ok(result);

        }



        /// <summary>
        /// گرفتن لیست سفارشهای ایجادشده توسط کاربر 
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserOrderB")]
        public async Task<ActionResult <OrderSelect>> UserOrderB()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimB = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });

            var currentUser = _db.CyUser.Where(x => x.IsVisible && x.ID == userId).FirstOrDefault();
            if (currentUser == null) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });
            List<CyOrder> orderList = new();

            var groupId = currentUser.CyGoroohID;
            var vahedId = currentUser.CyVahedID;

            var ordersQuery = _db.CyOrder
             .Where(x => x.IsVisible && x.CyUserID == userId)
             .Include(i => i.CyVahed)
             .Include(i => i.CyGorooh)
             .Include(i => i.CyUser)
             .AsQueryable();



            // مدیر کل و بقیه → بدون فیلتر

            var result = await ordersQuery.Select(s => new OrderSelect
            {
                id = s.ID,
                orderCode = s.OrderCode,
                cyVahed = s.CyVahed.Text,
                cyGorooh = s.CyGorooh.Text,
                createDate = s.CreateDate,
                cyUser = s.CyUser.CyUsNm,
                totalAmount = s.TotalAmount,
                orderStatus = s.Status,
                startOrderDate = s.LastModified ///زمان آغاز سفارش
            }).OrderByDescending(o => o.createDate).ToListAsync();

            return Ok(result);

        }



        /// <summary>
        /// لیست سفارشات قطعه سمت فروشنده
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserOrderC")]
        async public Task<ActionResult <OrderSelect>> UserOrderC(OrderStatus? status)
        {
            var userCliam = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userCliam, out Guid userId)) return Unauthorized(new { msg = "احراز هویت انجام نشده است ..." });
            if (userClaimRole == null) return BadRequest(new { msg = "نقش کاربر مشخص نیست" });

            UserType userType = UserTypeExtensionsB.GetRoleType(userClaimRole);

            if (userType != UserType.PurchasingExpert && userType != UserType.PurchasingManager) return BadRequest(new { msg = "شما دسترسی به این قسمت ندارید" });

            var ordersQuery = _db.CyOrder.AsNoTracking().Where(x => x.IsVisible).Include(i => i.CyUser).Include(i => i.CyVahed).Include(i => i.CyGorooh).AsQueryable();


            if (status == OrderStatus.Inquiring)
            {
                ordersQuery = ordersQuery.Where(x => x.Status == OrderStatus.Inquiring);
            }
            else if (status == OrderStatus.InOrderToSupply)
            {
                ordersQuery = ordersQuery.Where(x => x.Status == OrderStatus.InOrderToSupply);
            }
            else if (status == OrderStatus.InSupply)
            {
                ordersQuery = ordersQuery.Where(x => x.Status == OrderStatus.InSupply);
            }
            else if (status == null)
            {

                ordersQuery = ordersQuery.Where(x => x.Status != OrderStatus.Pending);
            }


            // مدیر کل و بقیه → بدون فیلتر

            var result = await ordersQuery.Select(s => new OrderSelect
            {
                id = s.ID,
                orderCode = s.OrderCode,
                cyVahed = s.CyVahed.Text,
                cyGorooh = s.CyGorooh.Text,
                createDate = s.CreateDate,
                cyUser = s.CyUser.CyUsNm,
                totalAmount = s.TotalAmount,
                orderStatus = s.Status,
                startOrderDate = s.LastModified, ///زمان آغاز سفارش
                curency=s.Curency
            }).OrderByDescending(o => o.createDate).ToListAsync();

            return Ok(result);


        }



        /// <summary>
        /// لیست سفارشات ارجاع شده به من 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getInviteOrders")]
        async public Task<ActionResult<OrderSelect>> getInviteOrders()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized();

            var userOrders = await _db.CyUserOrders.Where(x => x.UserID == userId).Select(s => s.OrderID).ToListAsync();

            var orders = _db.CyOrder.Where(x => userOrders.Contains(x.ID)).
                Include(i => i.CyVahed)
             .Include(i => i.CyGorooh)
             .Include(i => i.CyUser).AsQueryable();


            var result = await orders.Select(s => new OrderSelect
            {
                id = s.ID,
                orderCode = s.OrderCode,
                cyVahed = s.CyVahed.Text,
                cyGorooh = s.CyGorooh.Text,
                createDate = s.CreateDate,
                cyUser = s.CyUser.CyUsNm,
                totalAmount = s.TotalAmount,
                orderStatus = s.Status,
                startOrderDate = s.LastModified, ///زمان آغاز سفارش
                curency=s.Curency
            }).OrderByDescending(o => o.createDate).ToListAsync();

            return Ok(result);


        }

        /// <summary>
        ///  نمایش جزییات یک سفارش بدون ایتم سفارشها
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("getOrderDetails")]
        async public Task<ActionResult<OrderDTO>> getOrderDetails(Guid orderId)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });


            var curentOrder =await _db.CyOrder.AsNoTracking().Where(x => x.IsVisible && x.ID == orderId).FirstOrDefaultAsync();
            if (curentOrder == null) return BadRequest(new { msg = "سفارش پیدا نشد" });

            var dto=_mapper.Map<OrderDTO>(curentOrder);

            dto.StartOrderDate = curentOrder.LastModified; ///زمان آغاز سفارش

            return Ok(dto);




        }

        /// <summary>
        /// گرفتن جزییات یک سفارش
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("getOrderItems")]
        public async Task<ActionResult> orderItemsDetail(Guid orderId)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });


            var currentOrder = await _db.CyOrder.Where(x => x.IsVisible && x.ID == orderId).
                Include(u => u.CyOrderItems).ThenInclude(u => u.Approvals).FirstOrDefaultAsync();

            var dto = _mapper.Map<List<OrderItemDTO>>(currentOrder.CyOrderItems);

            //dto.OrderByDescending(o => o.CreateDate);

            return Ok(dto);

        }

        /// <summary>
        ///  صرفا جهت تست نمایش جزییات ایتم سفارش
        /// </summary>
        [HttpGet("getOrderItemDetail")]
        async public Task<ActionResult> getOrderItemDetail(Guid id)
        {
            var currentOrderitem = await _db.CyOrderItem.Where(x => x.IsVisible && x.ID == id).FirstOrDefaultAsync();
            if (currentOrderitem == null) return BadRequest(new { msg = "ایتم مورد نظر یافت نشد" });

            var dto = _mapper.Map<OrderItemDTO>(currentOrderitem);

            return Ok(dto);
        }

        /// <summary>
        /// تایید/عدم تایید گروهی آیتم سفارشهای یک سفارش
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("ApprovalCheckSelectAll")]
        public async Task<ActionResult> ApprovalCheckSelectAll(
    Guid id,
    string role,
    bool isFirest,
    bool isTrue)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userClaim, out Guid userId) || userRole == null)
                return Unauthorized();

            var currentUser = await _db.CyUser.Where(x => x.IsVisible && x.ID == userId).Select(x => new { x.ID, x.CyUsNm }).FirstOrDefaultAsync();

            UserType userType = UserTypeExtensionsB.GetRoleType(userRole);
            UserType inputUserType = UserTypeExtensionsB.GetRoleType(role);

            var currenOrder = await _db.CyOrder.Where(x => x.IsVisible && x.ID == id).FirstOrDefaultAsync();

            // 1️⃣ همه آیتم‌ها یکجا
            var orderItems = await _db.CyOrderItem.Where(x => x.IsVisible && x.CyOrderID == id).Select(x => new
            {
                x.ID,
                x.LevelOk,
                x.LevelBOk,
                x.UnitPrice
            })
                .ToListAsync();

            var cancelItem = orderItems.Where(x => x.UnitPrice == 0).Any();
            ///برای زمانی که ایتم سفارش توسط قسمت خرید قیمتگذاری نشده باشد
            if (currenOrder?.Status == OrderStatus.AwaitingCustomerApproval && cancelItem) return BadRequest(new { msg = "به علت لغو تعدادی از آیتمها انتخاب همه آیتمها امکان پذیر نمی باشد" });


            if (!orderItems.Any())
                return BadRequest("No items found");

            if (orderItems.Any(item =>
                (isFirest && userType < item.LevelOk) ||
                (!isFirest && userType < item.LevelBOk)))
            {
                return BadRequest(new { msg = "امکان ویرایش بعد از ثبت تغییرات وجود ندارد" });
            }

            var itemIds = orderItems.Select(x => x.ID).ToList();

            var approvals = await _db.OrderItemApproval.Where(x =>
                    itemIds.Contains(x.CyOrderItemID) &&
                    x.Role == inputUserType &&
                    x.IsFirst == isFirest)
                .ToListAsync();

            if (approvals.Count != itemIds.Count)
                return BadRequest("Some approvals not found");

            foreach (var approval in approvals)
            {
                approval.IsApproved = isTrue;
                approval.ActionTime = DateTime.Now;
                approval.CyUserID = userId;
                approval.UserName = currentUser.CyUsNm;
            }

            await _db.SaveChangesAsync();

            return Ok(new { msg = "All Items Selected" });
        }




        /// <summary>
        /// تایید/عدم تایید تک ایتم سفارش قبل  ازاستعلام گیری 
        /// </summary>

        [HttpGet("ApprovalCheck")]
        public async Task<ActionResult> ApprovalCheck(Guid id)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userClaim, out Guid userId) || userRole == null) return Unauthorized();



            var currentUser = await _db.CyUser.Where(x => x.IsVisible && x.ID == userId).FirstOrDefaultAsync();
            UserType userType = UserTypeExtensionsB.GetRoleType(userRole);

            var currentApproval = await _db.OrderItemApproval.Where(x => x.IsVisible && x.IsFirst == true && x.CyOrderItemID == id && x.Role == userType).FirstOrDefaultAsync();
            var currenOrderItem = await _db.CyOrderItem.Where(x => x.IsVisible && x.ID == id).Include(i => i.CyOrder).FirstOrDefaultAsync();
            if (currentApproval == null) return BadRequest(new { msg = "چک آیتمی با این آیدی یافت نشد" });

            if (currenOrderItem?.CyOrder?.Status == OrderStatus.Inquiring) return BadRequest(new { msg = "دراین مرحله مجاز به اعمال تغییر نیستید" });




            if (currenOrderItem.LevelOk == null || currenOrderItem.LevelOk <= userType)
            {
                currentApproval.IsApproved = !currentApproval.IsApproved;
                currentApproval.ActionTime = DateTime.Now;
                currentApproval.CyUserID = userId;
                currentApproval.UserName = currentUser.CyUsNm;
            }
            else
            {
                return BadRequest(new { msg = " پس از ثبت تغییرات امکان ویرایش وجود ندارد " });
            }

            await _db.SaveChangesAsync();
            return Ok(currentApproval.IsApproved);

        }


        /// <summary>
        /// تایید/عدم تایید تک ایتم سفارش بعد از استعلام گیری 
        /// </summary>
        [HttpGet("ApprovalCheckB")]
        public async Task<ActionResult> ApprovalCheckB(Guid id)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userClaim, out Guid userId) || userRole == null) return Unauthorized();



            var currentUser = await _db.CyUser.Where(x => x.IsVisible && x.ID == userId).FirstOrDefaultAsync();
            UserType userType = UserTypeExtensionsB.GetRoleType(userRole);

            var currentApproval = await _db.OrderItemApproval.Where(x => x.IsVisible && x.IsFirst == false && x.CyOrderItemID == id && x.Role == userType).FirstOrDefaultAsync();
            var currenOrderItem = await _db.CyOrderItem.Where(x => x.IsVisible && x.ID == id).Include(i => i.CyOrder).FirstOrDefaultAsync();
            if (currentApproval == null) return BadRequest(new { msg = "چک آیتمی با این آیدی یافت نشد" });

            if (currenOrderItem?.CyOrder?.Status == OrderStatus.Inquiring) return BadRequest(new { msg = "دراین مرحله مجاز به اعمال تغییر نیستید" });

            ///برای زمانی که ایتم سفارش توسط قسمت خرید قیمتگذاری نشده باشد
            if (currenOrderItem?.CyOrder?.Status == OrderStatus.AwaitingCustomerApproval && currenOrderItem.UnitPrice == 0) return BadRequest(new { msg = "این آیتم لغو شده است" });

            if (currenOrderItem.LevelBOk == null || currenOrderItem.LevelBOk <= userType)
            {
                currentApproval.IsApproved = !currentApproval.IsApproved;
                currentApproval.ActionTime = DateTime.Now;
                currentApproval.CyUserID = userId;
                currentApproval.UserName = currentUser.CyUsNm;
            }
            else
            {
                return BadRequest(new { msg = " پس از ثبت تغییرات امکان ویرایش وجود ندارد " });
            }

            await _db.SaveChangesAsync();
            return Ok(currentApproval.IsApproved);

        }

        ////جهت تایید نهایی هر نقض روی لیست  orderItemS
        [HttpGet("orderItemsOk")]
        async public Task<ActionResult> orderItemsOk(Guid OrderId)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userClaim, out Guid userId) || userRole == null) return Unauthorized();

            UserType userType = UserTypeExtensionsB.GetRoleType(userRole);
            if (userType != UserType.GroupExpert && userType != UserType.GroupManager && userType != UserType.DepartmentManager && userType != UserType.GeneralManager) return BadRequest(new { msg = "دسترسی به این بخش ندارید" });

            var currentOrder = _db.CyOrder.Where(x => x.IsVisible && x.ID == OrderId).Include(i => i.CyOrderItems).Include(i => i.CyUser).FirstOrDefault();
            if (currentOrder == null) return BadRequest("order Not Found");

            if (currentOrder.Status == OrderStatus.Pending)
            {
                foreach (var item in currentOrder.CyOrderItems)
                {
                    ///بررسی اینکه ایا ایتم سفارش تا به حال تایید شده و یا در چه سطحی تایید شده است
                    if (item.LevelOk == null || item.LevelOk < userType)
                    {

                        item.LevelOk = userType;

                    }

                }
            }
            else if (currentOrder.Status == OrderStatus.AwaitingCustomerApproval)
            {
                foreach (var item in currentOrder.CyOrderItems)
                {
                    ///بررسی اینکه ایا ایتم سفارش تا به حال تایید شده و یا در چه سطحی تایید شده است
                    if (item.LevelBOk == null || item.LevelBOk < userType)
                    {

                        item.LevelBOk = userType;

                    }

                }
            }


            var query = await _db.CyUser.Where(x => x.IsVisible).ToListAsync();

            var creatorUser = currentOrder.CyUser.CyUsNm;
            var checkerUser = query.Where(x => x.IsVisible && x.ID == userId).Select(s => s.CyUsNm).First();

            List<CyUser> userGuidList = new();

            if (currentOrder.CyGoroohId != null)
            {
                userGuidList.Add(query.Where(x => x.CyGoroohID == currentOrder.CyGoroohId && x.userType == UserType.GroupManager).First());
            }
            if (currentOrder.CyVahedId != null)
            {
                userGuidList.Add(query.Where(x => x.CyVahedID == currentOrder.CyVahedId && x.userType == UserType.DepartmentManager).First());
            }
            userGuidList.Add(query.Where(x => x.userType == UserType.GeneralManager).First());


            foreach (var item in userGuidList)
            {
                if (item.userType > userType)
                {
                    CyOrderNotification newNotif = new CyOrderNotification()
                    {
                        ID = new Guid(),
                        CreateDate = DateTime.Now,
                        IsVisible = true,
                        OrderID = OrderId,
                        SentDate = DateTime.Now,
                        Status = NotificationStatus.Sent,
                        Title = $"ثبت سفارش توسط  {creatorUser} ___ تایید شده توسط {checkerUser} ",
                        //Title =  "ثبت سفارش توسط " +creatorUser+ "تایید شده توسط " + checkerUser,
                        OrderType = OrderType.Parts,
                        ReceiverID = item.ID
                    };

                    await _db.CyOrderNotification.AddAsync(newNotif);
                }
            }

            await _db.SaveChangesAsync();
            return Ok(new { msg = "تغییرات ثبت شد" });



        }

        /// <summary>
        /// گرفتن جزییات تاییدیه
        /// </summary>
        [HttpGet("getApprovalDetail")]
        async public Task<ActionResult> getApprovalDetail(Guid id, string userTypeStrng)
        {
            UserType userType = UserTypeExtensionsB.GetRoleType(userTypeStrng);

            //UserType userType = userTypeStrng switch
            //{
            //    "GroupExpert" => UserType.GroupExpert,
            //    "GroupManager" => UserType.GroupManager,
            //    "DepartmentManager" => UserType.DepartmentManager,
            //    "GeneralManager" => UserType.GeneralManager,
            //    _ => UserType.NewUser
            //};

            var currentAppraval = await _db.OrderItemApproval.Where(x => x.CyOrderItemID == id && x.RoleName == userTypeStrng).Include(i => i.CyOrderItem).FirstOrDefaultAsync();
            if (currentAppraval == null) return BadRequest(new { msg = "آیتم یافت نشد" });

            var userId = currentAppraval.CyOrderItem.CreatorId;

            var orderItemcreator = await _db.CyUser.Where(x => x.IsVisible && x.ID == userId).FirstOrDefaultAsync();
            if (orderItemcreator == null) return BadRequest(new { msg = "نام کاربری یافت نشد" });


            var dto = _mapper.Map<OrderItemApprovalDTO>(currentAppraval);

            /// نقش و نام ایجادکننده ایتم سفارش
            dto.CreatorName = currentAppraval.CyOrderItem.CreatorName;
            dto.RoleName = UserTypeExtensions.GetRoleName(orderItemcreator.userType);

            return Ok(dto);
        }



        /// <summary>
        /// ویرایش ایتم سفارش ها در قسمت استعلامگیری توسط بخش خرید
        /// </summary>
        [HttpPut("updateOrderItem")]
        async public Task<ActionResult> updateOrderItem([FromBody] OrderItemDTO dto)
        {
            var currentOrderItem = await _db.CyOrderItem.Where(x => x.IsVisible && x.ID == dto.ID).Include(i=>i.CyOrder).FirstOrDefaultAsync();
            if (currentOrderItem == null) return BadRequest(new { msg = "سفارش یافت نشد" });


            //currentOrderItem = _mapper.Map<CyOrderItem>(dto);

            currentOrderItem.CyOrder.TotalAmount -= currentOrderItem.TotalPrice;

            currentOrderItem.CommentB = dto.CommentB;
            currentOrderItem.UnitPrice = dto.UnitPrice;
            currentOrderItem.TotalPrice =  Math.Round(currentOrderItem.Quantity * dto.UnitPrice, 3);
            currentOrderItem.Duration = dto.Duration;

            currentOrderItem.CyOrder.TotalAmount += currentOrderItem.TotalPrice;

            await _db.SaveChangesAsync();

            return Ok(new { msg = "ویرایش با موفقیت انجام شد" ,totalAmount = currentOrderItem.CyOrder.TotalAmount });
        }


        /// <summary>
        /// ویرایش وضعیت تامین  در قسمت در حال تامین سمت خرید
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("updateOrderItemSupplyStatus")]
        async public Task<ActionResult> updateOrderItemSupplyStatus([FromBody] OrderItemDTO dto)
        {
            var currentOrderItem = await _db.CyOrderItem.Where(x => x.IsVisible && x.ID == dto.ID).FirstOrDefaultAsync();
            if (currentOrderItem == null) return BadRequest(new { msg = "سفارش یافت نشد" });


            //currentOrderItem = _mapper.Map<CyOrderItem>(dto);

            currentOrderItem.InSupplyStatus = dto.InSupplyStatus;


            await _db.SaveChangesAsync();

            return Ok(new { msg = "ویرایش با موفقیت انجام شد" });
        }


        ////جهت تغییر وضعیت سمت  سفارش
        [HttpGet("changeOrderStatus")]
        async public Task<ActionResult> changeOrderStatus(Guid orderId)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var claimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId) || claimRole == null) return Unauthorized();

            var currentUserType = UserTypeExtensionsB.GetRoleType(claimRole);
            //var currentUserType = UserTypeExtensions.GetRoleName(claimRole);

            if (currentUserType != UserType.DepartmentManager && currentUserType != UserType.GeneralManager) return BadRequest(new { msg = "شما مجازبه انجام این فرآیند نیستید" });


            var currentOrder = _db.CyOrder.Where(x => x.IsVisible && x.ID == orderId).FirstOrDefault();
            if (currentOrder == null) return BadRequest(new { msg = "سفارش یافت نشد" });

            string title;

            if (currentOrder.Status == OrderStatus.Pending)
            {
                currentOrder.Status = OrderStatus.Inquiring;
                title = "تغییر وضعیت سفارش به : در حال استعلام گیری";
            }
            else if (currentOrder.Status == OrderStatus.AwaitingCustomerApproval)
            {
                currentOrder.Status = OrderStatus.InOrderToSupply;
                title = "تغییر وضعیت سفارش به : در صف خرید";
            }
            else
            {
                return BadRequest(new { msg = "مجاز به ثبت فرآیند نیستید" });
            }



            ////////////مشخص کردن سلسل مراتب سفارش ====>
            var query = await _db.CyUser.Where(x => x.IsVisible).ToListAsync();

            var creatorUser = currentOrder.CyUser.CyUsNm;
            var checkerUser = query.Where(x => x.IsVisible && x.ID == userId).Select(s => s.CyUsNm).First();

            List<CyUser> userGuidList = new();

            if (currentOrder.CyUserID != null)
            {
                ///بررسی اینکه آیا ایجادکننده سفارش  کارشناس گروه میباشد یا نه
                var creator = query.Where(x => x.IsVisible && x.ID == currentOrder.CyUserID && x.userType == UserType.GroupExpert).FirstOrDefault();
                if (creator != null)
                {
                    userGuidList.Add(creator);
                }
            }
            if (currentOrder.CyGoroohId != null)
            {
                userGuidList.Add(query.Where(x => x.CyGoroohID == currentOrder.CyGoroohId && x.userType == UserType.GroupManager).First());
            }
            if (currentOrder.CyVahedId != null)
            {
                userGuidList.Add(query.Where(x => x.CyVahedID == currentOrder.CyVahedId && x.userType == UserType.DepartmentManager).First());
            }
            userGuidList.Add(query.Where(x => x.userType == UserType.GeneralManager).First());


            ////اضافه کردن کارشناس خردید و مدیر خرید  به لیست اعلانات
            var PurchasingUsers = query.Where(x => x.userType == UserType.PurchasingExpert || x.userType == UserType.PurchasingManager).ToList();
            foreach (var user in PurchasingUsers)
            {
                userGuidList.Add(user);
            }


            foreach (var item in userGuidList)
            {

                CyOrderNotification newNotif = new CyOrderNotification()
                {
                    ID = new Guid(),
                    CreateDate = DateTime.Now,
                    IsVisible = true,
                    OrderID = orderId,
                    SentDate = DateTime.Now,
                    Status = NotificationStatus.Sent,
                    Title = $": {title} توسط : {checkerUser}",
                    //Title =  "ثبت سفارش توسط " +creatorUser+ "تایید شده توسط " + checkerUser,
                    OrderType = OrderType.Parts,
                    ReceiverID = item.ID
                };

                await _db.CyOrderNotification.AddAsync(newNotif);

            }


            await _db.SaveChangesAsync();


            return Ok(new { msg = "فرآیند با موفقیت ثبت شد" });
        }



        ////جهت تغییر وضعیت سفارش برای سمت خرید
        [HttpGet("changeOrderStatusB")]
        async public Task<ActionResult> changeOrderStatusB(Guid orderId)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var claimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId) || claimRole == null) return Unauthorized();

            var currentUserType = UserTypeExtensionsB.GetRoleType(claimRole);
            //var currentUserType = UserTypeExtensions.GetRoleName(claimRole);

            if (currentUserType != UserType.PurchasingExpert && currentUserType != UserType.PurchasingManager) return BadRequest(new { msg = "شما مجازبه انجام این فرآیند نیستید" });

            var currentOrder = _db.CyOrder.Where(x => x.IsVisible && x.ID == orderId).FirstOrDefault();
            if (currentOrder == null) return BadRequest(new { msg = "سفارش یافت نشد" });
            string title;

            if (currentOrder.Status == OrderStatus.Inquiring)
            {
                currentOrder.Status = OrderStatus.AwaitingCustomerApproval;
                title = "نغییر وضعیت سفارش به : در انتظار تایید مشتری";

            }
            else if ((currentUserType == UserType.PurchasingManager || currentUserType == UserType.PurchasingExpert) && currentOrder.Status == OrderStatus.InOrderToSupply)
            {
                currentOrder.Status = OrderStatus.InSupply;
                currentOrder.LastModified = DateTime.Now;///زمان غاز سفارش  
                title = "نغییر وضعیت سفارش به : درحال تامین";
            }
            else if ((currentUserType == UserType.PurchasingManager || currentUserType == UserType.PurchasingExpert) && currentOrder.Status == OrderStatus.InSupply)
            {
                currentOrder.Status = OrderStatus.Finished;
                currentOrder.DeliveredDate = DateTime.Now;
                title = "نغییر وضعیت سفارش به : خاتمه یافته";
            }
            else
            {
                return BadRequest(new { msg = "مجاز به ثبت فرآیند نیستید" });
            }


            ////////////مشخص کردن سلسل مراتب سفارش ====>
            var query = await _db.CyUser.Where(x => x.IsVisible).ToListAsync();

            var creatorUser = currentOrder.CyUser.CyUsNm;
            var checkerUser = query.Where(x => x.IsVisible && x.ID == userId).Select(s => s.CyUsNm).First();

            List<CyUser> userGuidList = new();

            if (currentOrder.CyUserID != null)
            {
                ///بررسی اینکه آیا ایجادکننده سفارش  کارشناس گروه میباشد یا نه
                var creator = query.Where(x => x.IsVisible && x.ID == currentOrder.CyUserID && x.userType == UserType.GroupExpert).FirstOrDefault();
                if (creator != null)
                {
                    userGuidList.Add(creator);
                }
            }
            if (currentOrder.CyGoroohId != null)
            {
                userGuidList.Add(query.Where(x => x.CyGoroohID == currentOrder.CyGoroohId && x.userType == UserType.GroupManager).First());
            }
            if (currentOrder.CyVahedId != null)
            {
                userGuidList.Add(query.Where(x => x.CyVahedID == currentOrder.CyVahedId && x.userType == UserType.DepartmentManager).First());
            }
            userGuidList.Add(query.Where(x => x.userType == UserType.GeneralManager).First());


            ////اضافه کردن کارشناس خردید و مدیر خرید  به لیست اعلانات
            var PurchasingUsers = query.Where(x => x.userType == UserType.PurchasingExpert || x.userType == UserType.PurchasingManager).ToList();
            foreach (var user in PurchasingUsers)
            {
                userGuidList.Add(user);
            }


            foreach (var item in userGuidList)
            {

                CyOrderNotification newNotif = new CyOrderNotification()
                {
                    ID = new Guid(),
                    CreateDate = DateTime.Now,
                    IsVisible = true,
                    OrderID = orderId,
                    SentDate = DateTime.Now,
                    Status = NotificationStatus.Sent,
                    Title = $": {title} توسط : {checkerUser}",
                    //Title =  "ثبت سفارش توسط " +creatorUser+ "تایید شده توسط " + checkerUser,
                    OrderType = OrderType.Parts,
                    ReceiverID = item.ID
                };

                await _db.CyOrderNotification.AddAsync(newNotif);

            }



            await _db.SaveChangesAsync();


            return Ok(new { msg = "فرآیند با موفقیت ثبت شد" });

        }




        /// <summary>
        /// نمایش جزییات ایجاد کننده ،تاریخ و کد پروژه 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="isPcb"></param>
        /// <returns></returns>
        [HttpGet("getOrderById")]
        async public Task<ActionResult> getOrderById(Guid orderId, bool isPcb)
        {
            if (isPcb == false)
            {
                var currentOrder = await _db.CyOrder.Where(x => x.IsVisible && x.ID == orderId).Include(i => i.CyUser).Select(s => new
                {
                    creatorName = s.CyUser.CyUsNm,
                    creatDate = s.CreateDate,
                    orderCode = s.OrderCode,
                }).FirstOrDefaultAsync();
                return Ok(currentOrder);
            }
            else
            {
                var currentPcb = await _db.CyPCBOrder.Where(x => x.IsVisible && x.ID == orderId).Include(i => i.CyUser).Select(s => new
                {
                    creatorName = s.CyUser.CyUsNm,
                    creatDate = s.CreateDate,
                    orderCode = s.OrderCode,
                }).FirstOrDefaultAsync();
                return Ok(currentPcb);
            }


        }


        /// <summary>
        /// سفارشات باز من 
        /// </summary>
        /// <returns></returns>
        [HttpGet("openOrder")]
        async public Task<ActionResult> openOrder()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimB = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });

            var currentUser = _db.CyUser.Where(x => x.IsVisible && x.ID == userId).FirstOrDefault();
            if (currentUser == null) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });
            List<CyOrder> orderList = new();

            var groupId = currentUser.CyGoroohID;
            var vahedId = currentUser.CyVahedID;

            var ordersQuery = _db.CyOrder
    .Where(x => x.IsVisible )
    .Include(i => i.CyVahed)
    .Include(i => i.CyGorooh)
    .Include(i => i.CyUser)
    .AsQueryable();

            if (currentUser.userType == UserType.GroupExpert)
            {
                ordersQuery = ordersQuery.Where(x => x.CyUserID == userId && (x.Status == OrderStatus.Pending || x.Status == OrderStatus.AwaitingCustomerApproval));
            }
            else if (currentUser.userType == UserType.GroupManager)
            {
                ordersQuery = ordersQuery.Where(x =>( x.CyUserID == userId || x.CyGoroohId == groupId) && (x.Status == OrderStatus.Pending || x.Status == OrderStatus.AwaitingCustomerApproval));
            }
            else if (currentUser.userType == UserType.DepartmentManager)
            {
                ordersQuery = ordersQuery.Where(x =>( x.CyUserID == userId || x.CyVahedId == vahedId) && (x.Status == OrderStatus.Pending || x.Status == OrderStatus.AwaitingCustomerApproval));

            }else if(currentUser.userType == UserType.PurchasingExpert || currentUser.userType == UserType.PurchasingManager)
            {
                ordersQuery = ordersQuery.Where(x => x.Status == OrderStatus.Inquiring || x.Status == OrderStatus.InSupply || x.Status == OrderStatus.InOrderToSupply);

            }



            // مدیر کل و بقیه → بدون فیلتر

            var result = await ordersQuery.Select(s => new
            {
                id = s.ID,
                orderCode = s.OrderCode,
                cyVahed = s.CyVahed.Text,
                cyGorooh = s.CyGorooh.Text,
                createDate = s.CreateDate,
                cyUser = s.CyUser.CyUsNm,
                totalAmount = s.TotalAmount,
                orderStatus = s.Status,
                startOrderDate = s.LastModified ///زمان آغاز سفارش
            }).OrderByDescending(o => o.createDate).ToListAsync();

            return Ok(result);

        }


        [HttpGet("cancelOrder")]
        async public Task<ActionResult> cancelOrder(Guid orderId, bool isPcb = false)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var claimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId) || claimRole == null) return Unauthorized();

            var query = await _db.CyUser.Where(x => x.IsVisible).ToListAsync();

            var checkerUser = query.Where(x => x.IsVisible && x.ID == userId).Select(s => s.CyUsNm).First();

            List<CyUser> userGuidList = new();



            if (isPcb == false)
            {
                var currentOrder = await _db.CyOrder.Where(x => x.IsVisible && x.ID == orderId).FirstOrDefaultAsync();
                if (currentOrder == null) return BadRequest("not Found");

                currentOrder.Status = OrderStatus.Canceled;
                currentOrder.CancelationDate = DateTime.Now;

                ////////////مشخص کردن سلسل مراتب سفارش ====>

                if (currentOrder.CyUserID != null)
                {
                    ///بررسی اینکه آیا ایجادکننده سفارش  کارشناس گروه میباشد یا نه
                    var creator = query.Where(x => x.IsVisible && x.ID == currentOrder.CyUserID && x.userType == UserType.GroupExpert).FirstOrDefault();
                    if (creator != null)
                    {
                        userGuidList.Add(creator);
                    }
                }
                if (currentOrder.CyGoroohId != null)
                {
                    userGuidList.Add(query.Where(x => x.CyGoroohID == currentOrder.CyGoroohId && x.userType == UserType.GroupManager).First());
                }
                if (currentOrder.CyVahedId != null)
                {
                    userGuidList.Add(query.Where(x => x.CyVahedID == currentOrder.CyVahedId && x.userType == UserType.DepartmentManager).First());
                }
                userGuidList.Add(query.Where(x => x.userType == UserType.GeneralManager).First());
                ////اضافه کردن کارشناس خردید و مدیر خرید  به لیست اعلانات
                var PurchasingUsers = query.Where(x => x.userType == UserType.PurchasingExpert || x.userType == UserType.PurchasingManager).ToList();
                foreach (var user in PurchasingUsers)
                {
                    userGuidList.Add(user);
                }

                foreach (var item in userGuidList)
                {

                    CyOrderNotification newNotif = new CyOrderNotification()
                    {
                        ID = new Guid(),
                        CreateDate = DateTime.Now,
                        IsVisible = true,
                        OrderID = orderId,
                        SentDate = DateTime.Now,
                        Status = NotificationStatus.Sent,
                        Title = $"لغو سفارش توسط : {checkerUser}",
                        //Title =  "ثبت سفارش توسط " +creatorUser+ "تایید شده توسط " + checkerUser,
                        OrderType = OrderType.Parts,
                        ReceiverID = item.ID
                    };

                    await _db.CyOrderNotification.AddAsync(newNotif);

                }


                _db.SaveChangesAsync();
                return Ok(new { msg = "سفارش لغو شد" });


            }

            //////////// pcb ===>
            else
            {
                var currentOrder = await _db.CyPCBOrder.Where(x => x.IsVisible && x.ID == orderId).FirstOrDefaultAsync();
                if (currentOrder == null) return BadRequest("not Found");
                currentOrder.Status = OrderStatus.Canceled;
                currentOrder.CancelationDate = DateTime.Now;

                ////////////مشخص کردن سلسل مراتب سفارش ====>

                if (currentOrder.CyUserID != null)
                {
                    ///بررسی اینکه آیا ایجادکننده سفارش  کارشناس گروه میباشد یا نه
                    var creator = query.Where(x => x.IsVisible && x.ID == currentOrder.CyUserID && x.userType == UserType.GroupExpert).FirstOrDefault();
                    if (creator != null)
                    {
                        userGuidList.Add(creator);
                    }
                }
                if (currentOrder.CyGoroohId != null)
                {
                    userGuidList.Add(query.Where(x => x.CyGoroohID == currentOrder.CyGoroohId && x.userType == UserType.GroupManager).First());
                }
                if (currentOrder.CyVahedId != null)
                {
                    userGuidList.Add(query.Where(x => x.CyVahedID == currentOrder.CyVahedId && x.userType == UserType.DepartmentManager).First());
                }
                userGuidList.Add(query.Where(x => x.userType == UserType.GeneralManager).First());


                ////اضافه کردن کارشناس خردید و مدیر خرید  به لیست اعلانات
                var PurchasingUsers = query.Where(x => x.userType == UserType.PurchasingExpert || x.userType == UserType.PurchasingManager).ToList();
                foreach (var user in PurchasingUsers)
                {
                    userGuidList.Add(user);
                }

                foreach (var item in userGuidList)
                {

                    CyOrderNotification newNotif = new CyOrderNotification()
                    {
                        ID = new Guid(),
                        CreateDate = DateTime.Now,
                        IsVisible = true,
                        OrderID = orderId,
                        SentDate = DateTime.Now,
                        Status = NotificationStatus.Sent,
                        Title = $"لغو سفارش توسط : {checkerUser}",
                        //Title =  "ثبت سفارش توسط " +creatorUser+ "تایید شده توسط " + checkerUser,
                        OrderType = OrderType.PCB,
                        ReceiverID = item.ID
                    };

                    await _db.CyOrderNotification.AddAsync(newNotif);

                }
                _db.SaveChangesAsync();
                return Ok(new { msg = "سفارش لغو شد" });
            }



        }



       /// <summary>
       /// تعیین سقف ارز مجاز 
       /// </summary>
       /// <param name="orderId"></param>
       /// <param name="curency"></param>
       /// <returns></returns>
        [HttpGet("setCurency")]
        async public Task<ActionResult> setCurency(Guid? orderId ,Currency curency)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimB = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });


            var currentOrder=await _db.CyOrder.Where(x=>x.IsVisible && x.ID == orderId).FirstOrDefaultAsync();
            if (currentOrder == null) return BadRequest();

            if(currentOrder.Status != OrderStatus.Inquiring) return BadRequest(new {msg="تعیین واحد ارز فقط در مرحله استعلام گیری امکان پذیر است"});

            currentOrder.Curency=curency;

            await _db.SaveChangesAsync();

            return Ok(new {msg=" واحد ارز با موفقیت ثبت شد"});


        }


        /// <summary>
        /// چک کردن مجاز بودن مدیر واحد جهت تایید سفارش 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("isAllowToDepartment")]
        async public Task<ActionResult> isAllowToDepartment(Guid orderId)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimB = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });

            if (UserTypeExtensionsB.GetRoleType(userClaimB) != UserType.DepartmentManager) return BadRequest("");

            var curentOrder=_db.CyOrder.Where(x=>x.IsVisible && x.ID == orderId).FirstOrDefault();
            if(curentOrder == null) return BadRequest();    

            if (curentOrder.Status == OrderStatus.Pending) return Ok(new { msg = "✔️" });
            var totlaAmount=curentOrder.TotalAmount;

           var intCurency= CurencyToInt.GetCurencyType((Currency) curentOrder.Curency);


            var curentCurency=await _db.CyKeyData.Where(x=>x.IsVisible && x.IntValue == intCurency).FirstOrDefaultAsync();

            var doubleValue = curentCurency.DoubleValue;


            if (totlaAmount <= doubleValue ) return Ok(new {msg="✔️"});

            return BadRequest(new { msg = "مبلغ سفارش بیشتر از حد مجاز می باشد." });


        }

    }
}
