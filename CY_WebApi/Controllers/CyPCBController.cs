using System.Security.Claims;
using AutoMapper;
using CY_BM;
using CY_DM;
using CY_WebApi.Migrations;
using CY_WebApi.Models;
using CY_WebApi.Services;
using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyPCBController : ControllerBase
    {


        private readonly CyContext _db;
        private readonly IMapper _mapper;

        public CyPCBController(CyContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

        }


        [HttpPost("addPcbFile")]
        async public Task<ActionResult> addPcbFile([FromBody] PCBFileDTO dto)
        {
            var userCliam = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userCliam, out Guid userId)) return Unauthorized(new { msg = "احراز هویت انجام نشده است ..." });
            if (userClaimRole == null) return BadRequest(new { msg = "نقش کاربر مشخص نیست" });

            var newFile = _mapper.Map<PCBFile>(dto);

            await _db.PCBFile.AddAsync(newFile);

            await _db.SaveChangesAsync();

            return Ok(newFile.ID);
        }


        [HttpPost("addPcb")]
        async public Task<ActionResult> addPcb([FromBody] PCBBDTO dto)
        {
            var userCliam = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userCliam, out Guid userId)) return Unauthorized(new { msg = "احراز هویت انجام نشده است ..." });
            if (userClaimRole == null) return BadRequest(new { msg = "نقش کاربر مشخص نیست" });


            var currentUser = await _db.CyUser.AsNoTracking().Where(x => x.IsVisible && x.ID == userId).Select(s => s.CyUsNm).FirstOrDefaultAsync();
            if (currentUser == null)
                return Unauthorized(new { msg = "کاربر معتبر نیست" });
            UserType userType = UserTypeExtensionsB.GetRoleType(userClaimRole);

            List<UserType> userTypeS = new(){
                UserType.GroupExpert,UserType.GroupManager ,UserType.DepartmentManager,UserType.GeneralManager
            };


            var newPcb = _mapper.Map<CyPCBOrder>(dto);
            newPcb.ID = Guid.NewGuid();

            newPcb.CreatorName = currentUser;
            newPcb.OrderDate = DateTime.Now;
            newPcb.Status = OrderStatus.Pending;
            newPcb.CyUserID = userId;

            var guidList = dto.GuidFileList;
            var currentFiles = await _db.PCBFile.Where(x => guidList.Contains(x.ID) && x.IsVisible).ToListAsync();
            if (currentFiles == null || currentFiles.Count < 3) return BadRequest(new { msg = "لیست آی دی ها کامل نیست" });


            foreach (var file in currentFiles)
            {
                file.CyPCBOrderID = newPcb.ID;
                file.CyUserID = userId;
            }


            foreach (var item1 in userTypeS)
            {
                newPcb.Approvals.Add(new PCBOrderItemApproval
                {
                    CyPCBOrderID = newPcb.ID,
                    Role = item1,
                    RoleName = item1.GetRoleName(),
                    IsApproved = item1 == userType ? true : false,
                    IsFirst = true,
                });
                newPcb.Approvals.Add(new PCBOrderItemApproval
                {
                    CyPCBOrderID = newPcb.ID,
                    Role = item1,
                    RoleName = item1.GetRoleName(),
                    IsApproved = false,
                    IsFirst = false,
                });
            }
            #region PcbNumber
            int nextPcbNumber =
                (await _db.CyPCBOrder.MaxAsync(x => (int?)x.PcbNumber)) ?? 0;
            nextPcbNumber++;
            #endregion
            newPcb.PcbNumber = nextPcbNumber; ;

            await _db.CyPCBOrder.AddAsync(newPcb);

            await _db.SaveChangesAsync();


            return Ok(new { msg = "سفارش با موفقیت ثبت شد" });


        }


        /// <summary>
        /// لیست سفارشهای پی سی بی ایجادشدهتوسط کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet("getUserPcbList")]
        async public Task<ActionResult<OrderPcbSelect>> getUserPcbList()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized();

            var pcbOrders = await _db.CyPCBOrder.AsNoTracking().Where(x => x.IsVisible && x.CyUserID == userId).
                Include(i => i.CyVahed).Include(i => i.CyGorooh).Select(s => new OrderPcbSelect
            {
                id = s.ID,
                OrderDate = s.OrderDate,
                PcbNumber = s.PcbNumber,
                OrderCode = s.OrderCode,
                cyVahed = s.CyVahed.Text,
                cyGorooh = s.CyGorooh.Text,
                TotalAmount = s.TotalAmount,
                CreatorName = s.CreatorName,
                Duration = s.Duration,
                Status = s.Status,

            }).OrderByDescending(o => o.OrderDate).ToListAsync();


            return Ok(pcbOrders);
        }


        /// <summary>
        /// لیست سفارشات پی سی بی زیرمجموعه کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet("getUserPcbListB")]
        async public Task<ActionResult<OrderPcbSelect>> getUserPcbListB()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized();

            var currentUser = _db.CyUser.Where(x => x.IsVisible && x.ID == userId).FirstOrDefault();
            if (currentUser == null) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });
            var groupId = currentUser.CyGoroohID;
            var vahedId = currentUser.CyVahedID;

            var ordersQuery = _db.CyPCBOrder.AsNoTracking().Where(x => x.IsVisible).Include(i => i.CyUser).Include(i => i.CyVahed).Include(i => i.CyGorooh).AsQueryable();

            if (currentUser.userType == UserType.GroupExpert)
            {
                ordersQuery = ordersQuery.Where(x => x.CyGoroohId == groupId);
            }
            else if (currentUser.userType == UserType.GroupManager)
            {
                ordersQuery = ordersQuery.Where(x => x.CyGoroohId == groupId);
            }
            else if (currentUser.userType == UserType.DepartmentManager)
            {
                ordersQuery = ordersQuery.Where(x => x.CyVahedId == vahedId);
            } else if (currentUser.userType == UserType.PurchasingExpert || currentUser.userType == UserType.PurchasingManager)
            {
                return Unauthorized();
            }



            // مدیر کل و بقیه → بدون فیلتر

            var result = await ordersQuery.Select(s => new OrderPcbSelect
            {
                id = s.ID,
                OrderDate = s.OrderDate,
                PcbNumber = s.PcbNumber,
                OrderCode = s.OrderCode,
                cyVahed = s.CyVahed.Text,
                cyGorooh = s.CyGorooh.Text,
                TotalAmount = s.TotalAmount,
                CreatorName = s.CreatorName,
                Duration = s.Duration,
                Status = s.Status,
            }).OrderByDescending(o => o.OrderDate).ToListAsync();

            return Ok(result);


        }


        /// <summary>
        /// لیست سفارشات پی سی بی سمتفروشنده
        /// </summary>
        /// <returns></returns>
        [HttpGet("getUserPcbListC")]
        async public Task<ActionResult<OrderPcbSelect>> getUserPcbListC(OrderStatus? status)
        {
            var userCliam = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userCliam, out Guid userId)) return Unauthorized(new { msg = "احراز هویت انجام نشده است ..." });
            if (userClaimRole == null) return BadRequest(new { msg = "نقش کاربر مشخص نیست" });

            UserType userType = UserTypeExtensionsB.GetRoleType(userClaimRole);

            if (userType != UserType.PurchasingExpert && userType != UserType.PurchasingManager) return BadRequest(new {msg="شما دسترسی به این قسمت ندارید"});

            var ordersQuery = _db.CyPCBOrder.AsNoTracking().Where(x => x.IsVisible).Include(i => i.CyUser).Include(i => i.CyVahed).Include(i => i.CyGorooh).AsQueryable();


            if (status == OrderStatus.Inquiring)
            {
                ordersQuery = ordersQuery.Where(x => x.Status == OrderStatus.Inquiring);
            }
            else if (status == OrderStatus.InOrderToSupply) {
                ordersQuery = ordersQuery.Where(x => x.Status == OrderStatus.InOrderToSupply);
            }
            else if (status == OrderStatus.InSupply)
            {
                ordersQuery = ordersQuery.Where(x => x.Status == OrderStatus.InSupply);
            }
            else if (status != OrderStatus.Pending ) {

                ordersQuery = ordersQuery.Where(x => x.Status != OrderStatus.Pending );
            }


            // مدیر کل و بقیه → بدون فیلتر

            var result = await ordersQuery.Select(s => new OrderPcbSelect
            {
                id = s.ID,
                OrderDate = s.OrderDate,
                PcbNumber = s.PcbNumber,
                OrderCode = s.OrderCode,
                cyVahed = s.CyVahed.Text,
                cyGorooh = s.CyGorooh.Text,
                TotalAmount = s.TotalAmount,
                CreatorName = s.CreatorName,
                Duration = s.Duration,
                Status = s.Status,
            }).OrderByDescending(o => o.OrderDate).ToListAsync();

            return Ok(result);


        }


        /// <summary>
        /// لیست سفارشات ارجاع شده به من 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getInviteOrders")]
        async public Task<ActionResult <OrderPcbSelect>> getInviteOrders()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized();

            var userOrders = await _db.CyUserOrders.Where(x => x.UserID == userId).Select(s => s.OrderID).ToListAsync();

            var orders = _db.CyPCBOrder.Where(x => userOrders.Contains(x.ID)).
                Include(i => i.CyVahed)
             .Include(i => i.CyGorooh)
             .Include(i => i.CyUser).AsQueryable();


            var result = await orders.Select(s => new OrderPcbSelect
            {
                id = s.ID,
                OrderDate = s.OrderDate,
                PcbNumber = s.PcbNumber,
                OrderCode = s.OrderCode,
                cyVahed = s.CyVahed.Text,
                cyGorooh = s.CyGorooh.Text,
                TotalAmount = s.TotalAmount,
                CreatorName = s.CreatorName,
                Duration = s.Duration,
                Status = s.Status,

            }).OrderByDescending(o => o.OrderDate).ToListAsync();

            return Ok(result);


        }





        /// <summary>
        /// جزییات یک سفارش با آی دی
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getPcbById")]
        async public Task<ActionResult> getPcbById(Guid id)
        {
            var userCliam = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userCliam, out Guid userId)) return Unauthorized(new { msg = "احراز هویت انجام نشده است ..." });

            var currentPcb = await _db.CyPCBOrder.AsNoTracking().Where(x => x.IsVisible && x.ID == id).Include(i => i.Approvals).Include(i => i.PCBFile).Include(i => i.CyUser).FirstOrDefaultAsync();
            if (currentPcb == null) return BadRequest(new { msg = "سفارش پیدا نشد" });

            var listId = currentPcb.PCBFile.Select(s => s.CyUserID).ToList();
            var users = await _db.CyUser.Where(x => listId.Contains(x.ID)).ToListAsync();

            var dtoFiles = _mapper.Map<List<PCBFileDTO>>(currentPcb.PCBFile);
            dtoFiles.OrderByDescending(o => o.CreateDate);

            foreach (var item in dtoFiles)
            {
                var userName = users.Where(x =>x.ID == item.CyUserID).Select(s => s.CyUsNm).FirstOrDefault();
                if (userName == null) return BadRequest();

                item.userName = userName;

            }


            var dto = _mapper.Map<PCBBDTO>(currentPcb);

            dto.PCBFileDTO = dtoFiles;

            return Ok(dto);

        }


        /// <summary>
        /// get PCBFileType by type and pcbId for test
        /// </summary>
        /// <param name="pcbId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("getPcbFileByType")]
        async public Task<ActionResult> getPcbFileByType(Guid pcbId, PCBFileType type)
        {
            var pcbFile = await _db.PCBFile.AsNoTracking().Where(x => x.IsVisible && x.CyPCBOrderID == pcbId && x.Type == type).Include(i => i.CyUser).Select(s => new
            {
                id = s.ID,
                SendDate = s.SendDate,
                CreateDate = s.CreateDate,
                CreatoName = s.CyUser.CyUsNm,
                FileID = s.FileID,
                Type = s.Type
            }).OrderByDescending(o => o.CreateDate).ToListAsync();
            if (!pcbFile.Any()) return BadRequest(new { msg = "فایلی پیدا نشد" });

            //var dto = _mapper.Map<List<PCBFileDTO>>(pcbFile);


            return Ok(pcbFile);

        }


        /// <summary>
        /// افزودن فایل به سفارش در مرحله ویرایش سفارش
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        [HttpPost("addPcbInEditPage")]
        async public Task<ActionResult> addPcbInEditPage([FromBody] PCBFileDTO dto)
        {
            var userCliam = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userCliam, out Guid userId)) return Unauthorized(new { msg = "احراز هویت انجام نشده است ..." });
            var currentUser = _db.CyUser.AsNoTracking().Where(x => x.IsVisible && x.ID == userId).Select(s => s.CyUsNm).FirstOrDefault();

            if (userClaimRole == null) return BadRequest(new { msg = "نقش کاربر مشخص نیست" });
            UserType userType = UserTypeExtensionsB.GetRoleType(userClaimRole);
            var currentPcb = _db.CyPCBOrder.Where(x => x.IsVisible && x.ID == dto.CyPCBOrderID).FirstOrDefault();
            if (currentPcb == null) return BadRequest(new { msg = " سفارش پیدا نشد" });

            if ((currentPcb.LevelOk == null || currentPcb.LevelOk <= userType) && currentPcb.Status == OrderStatus.Pending)
            {
                dto.CyUserID = userId; ////ایجاد کننده فایل
                dto.userName = currentUser;
                var newFile = _mapper.Map<PCBFile>(dto);


                await _db.PCBFile.AddAsync(newFile);
                await _db.SaveChangesAsync();
                return Ok(new { msg = "" });

            }


            return BadRequest(new { msg = "در این مرحله امکان افزودن فایل جدید وجود ندارد" });





        }


        [HttpGet("ApprovalCheck")]
        public async Task<ActionResult> ApprovalCheck(Guid id)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userClaim, out Guid userId) || userRole == null) return Unauthorized();



            var currentUser = await _db.CyUser.Where(x => x.IsVisible && x.ID == userId).FirstOrDefaultAsync();
            UserType userType = UserTypeExtensionsB.GetRoleType(userRole);

            var currentApproval = await _db.PCBOrderItemApproval.Where(x => x.IsVisible && x.ID == id).Include(i => i.CyPCBOrder).FirstOrDefaultAsync();
            var pcbId = currentApproval.CyPCBOrderID;
            var currentPcb = await _db.CyPCBOrder.Where(x => x.IsVisible && x.ID == pcbId).FirstOrDefaultAsync();
            if (currentApproval == null) return BadRequest(new { msg = "چک آیتمی با این آیدی یافت نشد" });

            if (currentPcb.Status == OrderStatus.Inquiring) return BadRequest(new { msg = "دراین مرحله مجاز به اعمال تغییر نیستید" });




            if ((currentPcb.LevelOk == null || currentPcb.LevelOk <= userType) && currentPcb.Status==OrderStatus.Pending)
            {
                currentApproval.IsApproved = !currentApproval.IsApproved;
                currentApproval.ActionTime = DateTime.Now;
                currentApproval.CyUserID = userId;///تایید کننده
                currentApproval.UserName = currentUser.CyUsNm;
            }
            else if ((currentPcb.LevelBOk == null || currentPcb.LevelBOk <= userType) && currentPcb.Status == OrderStatus.AwaitingCustomerApproval)
            {
                currentApproval.IsApproved = !currentApproval.IsApproved;
                currentApproval.ActionTime = DateTime.Now;
                currentApproval.CyUserID = userId;///تایید کننده
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

            var currentOrder = _db.CyPCBOrder.Where(x => x.IsVisible && x.ID == OrderId).Include(i => i.PCBFile).FirstOrDefault();
            if (currentOrder == null) return BadRequest("order Not Found");

            var currentPcbFiles = currentOrder.PCBFile;

            if (currentOrder.Status == OrderStatus.Pending)
            {

                ///بررسی اینکه ایا ایتم سفارش تا به حال تایید شده و یا در چه سطحی تایید شده است
                if (currentOrder.LevelOk == null || currentOrder.LevelOk <= userType)
                {

                    currentOrder.LevelOk = userType;

                    foreach (var item in currentPcbFiles)
                    {
                        if (item.SendDate == null)
                        {
                            item.SendDate = DateTime.Now;
                        }

                    }

                }

            }
            else if (currentOrder.Status == OrderStatus.AwaitingCustomerApproval)
            {

                ///بررسی اینکه ایا ایتم سفارش تا به حال تایید شده و یا در چه سطحی تایید شده است
                if (currentOrder.LevelBOk == null || currentOrder.LevelBOk <= userType)
                {

                    currentOrder.LevelBOk = userType;
                    foreach (var item in currentPcbFiles)
                    {
                        if (item.SendDate == null)
                        {
                            item.SendDate = DateTime.Now;
                        }

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
                        OrderType = OrderType.PCB,
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
        async public Task<ActionResult> getApprovalDetail(Guid id)
        {
            var currentAppraval = await _db.PCBOrderItemApproval.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (currentAppraval == null) return BadRequest(new { msg = "آیتم یافت نشد" });

            var currentUser=_db.CyUser.AsNoTracking().Where(x=>x.IsVisible && x.ID == currentAppraval.CyUserID).Select(s=>s.CyUsNm).FirstOrDefault();

            var dto = _mapper.Map<PCBOrderItemApprovalDTO>(currentAppraval);

            /// نقش و نام ایجادکننده ایتم سفارش
            dto.UserName = currentUser;
            dto.RoleName = UserTypeExtensions.GetRoleName(currentAppraval.Role);

            return Ok(dto);
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

            var currentOrder = _db.CyPCBOrder.Where(x => x.IsVisible && x.ID == orderId).Include(i=>i.Approvals).FirstOrDefault();
            if (currentOrder == null) return BadRequest(new { msg = "سفارش یافت نشد" });


            ////جهت بررسی سقف مجاز جهت تایید مدیر واحد
            if (currentUserType == UserType.DepartmentManager)
            {
                var intCurency = CurencyToInt.GetCurencyType((Currency)currentOrder.Currency);
                var curentCurency = await _db.CyKeyData.Where(x => x.IsVisible && x.IntValue == intCurency).FirstOrDefaultAsync();

                var doubleValue = curentCurency.DoubleValue;

                if (currentOrder.TotalAmount > doubleValue) return BadRequest(new { msg = "مبلغ سفارش بیشتر از حد مجاز می باشد." });


            }
                
            string title;

            if (currentOrder.Status == OrderStatus.Pending)
            {
                var departmentApproval=currentOrder.Approvals.Where(x=>x.IsFirst==true && x.Role==UserType.DepartmentManager).FirstOrDefault();
                var generalApproval=currentOrder.Approvals.Where(x=>x.IsFirst==true && x.Role==UserType.GeneralManager).FirstOrDefault();
                if ((currentUserType == UserType.DepartmentManager && departmentApproval.IsApproved == false) || (currentUserType == UserType.GeneralManager && generalApproval.IsApproved == false))
                    return BadRequest(new { msg = "برای تغییر وضعیت سفارش ابتدا باید تاییدیه  مربوط به خود را ثبت نمایید " });

                currentOrder.SendDate = DateTime.Now;///تاریخ شروع فرآیند سفارش
                currentOrder.Status = OrderStatus.Inquiring;
                title = "نغییر وضعیت سفارش به : درحال استعلام گیری";
            }
            else if (currentOrder.Status == OrderStatus.AwaitingCustomerApproval)
            {
                var departmentApproval = currentOrder.Approvals.Where(x => x.IsFirst == false && x.Role == UserType.DepartmentManager).FirstOrDefault();
                var generalApproval = currentOrder.Approvals.Where(x => x.IsFirst == false && x.Role == UserType.GeneralManager).FirstOrDefault();
                if ((currentUserType == UserType.DepartmentManager && departmentApproval.IsApproved == false) || (currentUserType == UserType.GeneralManager && generalApproval.IsApproved == false))
                    return BadRequest(new { msg = "برای تغییر وضعیت سفارش ابتدا باید تاییدیه  مربوط به خود را ثبت نمایید " });
                currentOrder.VerifiedDate = DateTime.Now;///تاریخ تایید سفارش
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
                    OrderType = OrderType.PCB,
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

            var currentOrder = _db.CyPCBOrder.Where(x => x.IsVisible && x.ID == orderId).FirstOrDefault();
            if (currentOrder == null) return BadRequest(new { msg = "سفارش یافت نشد" });
            
            string title;

            if (currentOrder.Status == OrderStatus.Inquiring)
            {
                currentOrder.Status = OrderStatus.AwaitingCustomerApproval;
                currentOrder.InquiryDate = DateTime.Now;
                title = "نغییر وضعیت سفارش به : در انتظار تایید مشتری";
            }
            else if ((currentUserType == UserType.PurchasingManager || currentUserType == UserType.PurchasingExpert) && currentOrder.Status == OrderStatus.InOrderToSupply)
            {
                currentOrder.Status = OrderStatus.InSupply;
                currentOrder.StartDate=DateTime.Now;///زمان غاز سفارش  
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
                    OrderType = OrderType.PCB,
                    ReceiverID = item.ID
                };

                await _db.CyOrderNotification.AddAsync(newNotif);

            }



            await _db.SaveChangesAsync();


            return Ok(new { msg = "فرآیند با موفقیت ثبت شد" });

        }



        /// <summary>
        /// ویرایش سفارش  در قسمت استعلامگیری توسط بخش خرید
        /// </summary>
        [HttpPut("updateOrderItem")]
        async public Task<ActionResult> updateOrderItem([FromBody] pcbDtoA dto)
        {
            var currentPcb = await _db.CyPCBOrder.Where(x => x.IsVisible && x.ID == dto.ID).FirstOrDefaultAsync();
            if (currentPcb == null) return BadRequest(new { msg = "سفارش یافت نشد" });

            if (dto.Amount == 0 || dto.Duration == null) return BadRequest(new { msg = "زمان تامین و قیمت باید معین شود" });

            currentPcb.CommentB = dto.CommentB;
            currentPcb.Duration = dto.Duration;
            currentPcb.Currency = dto.Currency;
            currentPcb.Amount = dto.Amount;
            currentPcb.TotalAmount = currentPcb.Count * dto.Amount;


            await _db.SaveChangesAsync();

            return Ok(new { msg = "ویرایش با موفقیت انجام شد" });
        }



        /// <summary>
        /// ویرایش وضعیت تامین  در قسمت در حال تامین سمت خرید
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("updateOrderItemSupplyStatus")]
        async public Task<ActionResult> updateOrderItemSupplyStatus(string state,Guid orderId)
        {
            var currentPcb = await _db.CyPCBOrder.Where(x => x.IsVisible && x.ID == orderId).FirstOrDefaultAsync();
            if (currentPcb == null) return BadRequest(new { msg = "سفارش یافت نشد" });


            //currentOrderItem = _mapper.Map<CyOrderItem>(dto);

            currentPcb.InSupplyStatus = state;


            await _db.SaveChangesAsync();

            return Ok(new { msg = "ویرایش با موفقیت انجام شد" });
        }



        /// <summary>
        /// سفارشات باز من pcb
        /// </summary>
        /// <returns></returns>
        [HttpGet("openPcbOrder")]
        async public Task<ActionResult <OrderSelect>> openOrder()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimB = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });

            var currentUser = _db.CyUser.Where(x => x.IsVisible && x.ID == userId).FirstOrDefault();
            if (currentUser == null) return Unauthorized(new { msg = "دسترسی به این قسمت امکان پذیر نیست" });
            List<CyOrder> orderList = new();

            var groupId = currentUser.CyGoroohID;
            var vahedId = currentUser.CyVahedID;

            var ordersQuery = _db.CyPCBOrder
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
                ordersQuery = ordersQuery.Where(x => (x.CyUserID == userId || x.CyGoroohId == groupId )&& (x.Status == OrderStatus.Pending || x.Status == OrderStatus.AwaitingCustomerApproval));
            }
            else if (currentUser.userType == UserType.DepartmentManager)
            {
                ordersQuery = ordersQuery.Where(x => (x.CyUserID == userId || x.CyVahedId == vahedId) && (x.Status == OrderStatus.Pending || x.Status == OrderStatus.AwaitingCustomerApproval));
            }else if(currentUser.userType == UserType.PurchasingExpert || currentUser.userType == UserType.PurchasingManager)
            {
                ordersQuery = ordersQuery.Where(x => x.Status == OrderStatus.Inquiring || x.Status == OrderStatus.InSupply || x.Status == OrderStatus.InOrderToSupply);
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
                startOrderDate = s.LastModified ///زمان آغاز سفارش
            }).OrderByDescending(o => o.createDate).ToListAsync();

            return Ok(result);

        }



        public class pcbDtoA
        {
            public virtual Guid? ID { get; set; }

            public double? Amount { get; set; }

            public Currency Currency { get; set; }

            public string? CommentB { get; set; }

            public string? Duration { get; set; }


        }

    }

    }
