using AutoMapper;
using CY_DM;
using CY_BM;
using CY_WebApi.DataAccess;
using CY_WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using CY_WebApi.Services;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using CY_WebApi.Migrations;

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyOrderMessageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Repository<CyOrderMessage> _repo;
        private readonly CyContext _db;
        private readonly SmsService _smsService;

        public CyOrderMessageController(CyContext context, IMapper mapper, SmsService smsService)
        {
            _mapper = mapper;
            _repo = new Repository<CyOrderMessage>(context);
            _db = context;
            _smsService = smsService;
        }


        //[Authorize]
        // [TypeAuthorize([UserType.Customer])]
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] OrderMessageDTO Message,bool isPcb=false)
        {

            var userid = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            if (userid == null)
                return Unauthorized("کاربر پیدا نشد");
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            UserType userType = UserTypeExtensionsB.GetRoleType(userClaimRole);

            var curenUser=_db.CyUser.Where(x=>x.IsVisible && x.ID == userid).Select(s=>s.CyUsNm).FirstOrDefault();
            //var userOrder = _db.CyUserOrders.Where(o => o.OrderID == Message.OrderID && o.UserID == userid).FirstOrDefault();
            //if (userOrder == null && userType!= UserType.GeneralManager ) BadRequest(new {msg="user Not Found in UserOrder List"});

            var NewMessage = new CyOrderMessage()
            {
                //Title = Message.Title,
                ID = Guid.NewGuid(),
                SenderID = userid,
                OrderID = Message.OrderID,
                Message = Message.Message,
                SentDate = DateTime.Now,
                FileID = Message.FileID != null ? Message.FileID : null,
                CreateDate = DateTime.Now,
                IsVisible = true,
                Mentions = Message.Mentions,
                MessageType = Message.MessageType,
                OrderType = Message.OrderType,
            };
            await _repo.Insert(NewMessage);

            foreach (var item in Message.Mentions)
            {
  
                    CyOrderNotification newNotif = new CyOrderNotification()
                    {
                        ID = new Guid(),
                        CreateDate = DateTime.Now,
                        IsVisible = true,
                        OrderID = Message.OrderID,
                        SentDate = DateTime.Now,
                        Status = NotificationStatus.Sent,
                        Title = $"ارسال پیام توسط : {curenUser}",
                        //Title =  "ثبت سفارش توسط " +creatorUser+ "تایید شده توسط " + checkerUser,
                        OrderType = isPcb==false? OrderType.Parts : OrderType.PCB ,
                        ReceiverID = item
                    };

                    await _db.CyOrderNotification.AddAsync(newNotif);
            }

            await _db.SaveChangesAsync();
            return Ok(new { msg = "پیام با موفقیت ارسال شد" });
        }

        //[Authorize]
        //[TypeAuthorize([UserType.Customer])]
        [HttpGet("GetMessagesByOrderID")]
        public async Task<IActionResult> GetMessagesByOrderID(Guid OrderID, OrderMessageType? type)
        {
            var identity = User.Identities.FirstOrDefault();
            if (identity == null)
                return Unauthorized("کاربر پیدا نشد");
            var userid = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            // var seluser = await _db.CyUser.FindAsync(userid);




            var query = _repo.Table.Where(m => m.IsVisible &&
                                                        m.OrderID == OrderID)
                                                        .Select(c =>
                                                        new OrderMessageBDTO()
                                                        {
                                                            ID = c.ID,
                                                            Message = c.Message,
                                                            FileID = c.FileID,
                                                            Mentions = _db.CyUser
                                                            .Where(u => c.Mentions.Contains(u.ID))
                                                            .Select(u => u.CyUsNm)
                                                            .ToList(),
                                                            MessageType = c.MessageType,
                                                            OrderType = c.OrderType,
                                                            SenderID = c.SenderID,
                                                            OrderID = c.OrderID,
                                                            SentDate = c.SentDate,
                                                            SenderName = c.Sender.CyUsNm,

                                                        }).OrderByDescending(o => o.SentDate).AsQueryable();
            if (type != null)
            {
                var faramoj_novin = await query.Where(x => x.MessageType == type).ToListAsync();

                return Ok(faramoj_novin);

            }
            //foreach (var message in OrderMessages)
            //{
            //    if (message.SenderID != userid && message.Status != OrderMessageStatus.Seen)
            //    {
            //        message.Status = OrderMessageStatus.Seen;
            //        message.SeenDate = DateTime.Now;
            //    }
            //}
            //await _repo.SaveChangesAsync();
            //var messages = _mapper.Map<ICollection<OrderMessageDTO>>(OrderMessages);
            //foreach (var message in messages)
            //{
            //    message.SenderName = seluser?.CyUsNm;
            //}
            //var order = await _db.CyOrder.Where(o => o.ID == OrderID && o.IsVisible).Include(o => o.CyAddress).FirstOrDefaultAsync();
            //var finalResu = new GetMessagesReturnDTO()
            //{
            //    OrderStatusStatusEnum = order?.Status,
            //    OrderStatus = order?.StatusText,
            //    File = order?.BomFileID != null ? order.BomFileID.Value : null,
            //    OrderDate = order?.OrderDate != null ? order.OrderDate : order != null ? order.CreateDate : null,
            //    //Address = order?.CyAddress?.Address,
            //    TotalAmount = order?.TotalAmount,
            //    Messages = messages,
            //};
            var OrderMessages = await query.ToListAsync();

            return Ok(OrderMessages);
        }





        [HttpGet("mention")]
        async public Task<ActionResult> mention(Guid orderId, bool isPcb = false)
        {

            var currentOrder = _db.CyOrder.Where(x => x.ID == orderId).FirstOrDefault();
            var currentPCBOrder = _db.CyPCBOrder.Where(x => x.ID == orderId).FirstOrDefault();


            if ((currentOrder == null && isPcb == false) || (currentPCBOrder == null && isPcb == true)) return BadRequest(new { msg = "order not fOund" });


            var groubId = isPcb == false ? currentOrder.CyGoroohId : currentPCBOrder.CyGoroohId;
            var vahedId = isPcb == false ? currentOrder.CyVahedId : currentPCBOrder.CyVahedId;
            var userList = await _db.CyUser.Where(x => (groubId != null && vahedId != null && (x.CyGoroohID == groubId && x.CyVahedID == vahedId)) ||
            (vahedId != null && x.userType == UserType.DepartmentManager && x.CyVahedID == vahedId) || x.userType == UserType.GeneralManager).Select(s => new
            {
                id = s.ID,
                name = s.CyUsNm,
                uerType = s.userType

            }).ToListAsync();

            return Ok(userList);

        }


        /// <summary>
        /// لیست افراد قابل دعوت
        /// </summary>
        /// <returns></returns>
        [HttpGet("invite")]
        public async Task<ActionResult> invite()
        {

            var userid = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            if (userid == null)
                return Unauthorized("کاربر پیدا نشد");
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            UserType userType = UserTypeExtensionsB.GetRoleType(userClaimRole);

            var query = _db.CyUser.AsNoTracking().
                Where(x => x.IsVisible && (x.userType == UserType.GroupExpert || x.userType == UserType.GroupManager || x.userType == UserType.DepartmentManager)).AsQueryable();

            if (userType == UserType.GeneralManager)
            {
                var allUsers = await query.Select(s => new
                {
                    id = s.ID,
                    name = s.CyUsNm,
                    uerType = s.userType

                }).ToListAsync();

                return Ok(allUsers);

            }
            else if (userType == UserType.GroupManager || userType == UserType.DepartmentManager)
            {
                var usersByType = await query.Where(x => x.userType == userType).Select(s => new
                {
                    id = s.ID,
                    name = s.CyUsNm,
                    uerType = s.userType

                }).ToListAsync();

                return Ok(usersByType);
            }

            return BadRequest(new { msg = "امکان دعوت برای شما وجود ندارد " });



        }


        /// <summary>
        /// افزودن به لیست دعوت
        /// </summary>
        /// <returns></returns>
        [HttpGet("addInvite")]
        public async Task<ActionResult> addInvite(Guid inviteUserId, Guid orderId, bool isPcb = false)
        {

            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized();
            var user = _db.CyUser.Where(x => x.IsVisible && x.ID == userId).Select(s => s.CyUsNm).FirstOrDefault();


            var currentUser = _db.CyUser.Where(x => x.IsVisible && x.ID == inviteUserId).FirstOrDefault();
            if (currentUser == null) return BadRequest(new { msg = "کاربر پیدانشد" });

            var currentOrder = _db.CyOrder.Where(x => x.IsVisible && x.ID == orderId).FirstOrDefault();
            var currentPCBOrder = _db.CyPCBOrder.Where(x => x.IsVisible && x.ID == orderId).FirstOrDefault();
            if ((currentOrder == null && isPcb == false) || (currentPCBOrder == null && isPcb == true)) return BadRequest(new { msg = "سفارش پیدانشد" });

            ////بررسی اینکه کاربر دعوت شده جز سلسله مراتب سفارش نباشد
            var orderCreator = isPcb == false ? currentOrder.CyUserID : currentPCBOrder.CyUserID;
            var groupId = isPcb == false ? currentOrder.CyGoroohId : currentPCBOrder.CyGoroohId;
            var vahedId = isPcb == false ? currentOrder.CyVahedId : currentPCBOrder.CyVahedId;
            var users = await _db.CyUser.AsNoTracking().
                Where(x => x.ID == orderCreator || (x.CyGoroohID == groupId && x.userType == UserType.GroupManager) || (x.CyVahedID == vahedId && x.userType == UserType.DepartmentManager) || x.userType == UserType.GeneralManager).
                Select(s => s.ID).ToListAsync();
            foreach (var item in users)
            {
                if (item == inviteUserId) return BadRequest(new { msg = "این کاربر در لیست گفتگو قرار دارد " });
            }
            var currentUserOrder = _db.CyUserOrders.Where(x => x.UserID == inviteUserId && x.OrderID == orderId).FirstOrDefault();

            if (currentUserOrder != null) return BadRequest(new { msg = "این کاربرقبلا به این گفتگو دعوت شده است" });

            var newUserOrdre = new CyUserOrder()
            {
                OrderID = orderId,
                UserID = inviteUserId
            };

            await _db.CyUserOrders.AddAsync(newUserOrdre);


            CyOrderNotification newNotif = new CyOrderNotification()
            {
                ID = new Guid(),
                CreateDate = DateTime.Now,
                IsVisible = true,
                OrderID = orderId,
                SentDate = DateTime.Now,
                Status = NotificationStatus.Sent,
                Title = $"دعوت به سفارش توسط : {user}",
                //Title =  "ثبت سفارش توسط " +creatorUser+ "تایید شده توسط " + checkerUser,
                OrderType = isPcb==false ? OrderType.Parts :OrderType.PCB,
                ReceiverID = inviteUserId
            };

            await _db.CyOrderNotification.AddAsync(newNotif);



            await _db.SaveChangesAsync();

            return Ok(new { msg = "دعوت از کاربر با موفقیت انجام شد" });


        }



        /// <summary>
        /// نمایش اعضای چت 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="isPcb"></param>
        /// <returns></returns>

        [HttpGet("getUsersOrderMessage")]
        public async Task<ActionResult> getUsersOrderMessage(Guid orderId, bool isPcb = false)
        {
            var order = await _db.CyOrder.Where(x => x.IsVisible && x.ID == orderId).FirstOrDefaultAsync();
            var pcb = await _db.CyPCBOrder.Where(x => x.IsVisible && x.ID == orderId).FirstOrDefaultAsync();
            if ((order == null && isPcb == false) || (pcb == null && isPcb == true)) return BadRequest("order not Found");

            List<userModel> usersList = new();

            var query = _db.CyUser.AsNoTracking().Where(x => x.IsVisible).AsQueryable();

            var groupId = isPcb == false ? order.CyGoroohId : pcb.CyGoroohId; ;
            var vahedId = isPcb == false ? order.CyVahedId : pcb.CyVahedId;

            if (groupId != null)///اضافه کردن مدیر گروه به لیست چت در 
            {
                var groupMang = await query.Where(x => x.CyGoroohID == groupId && x.userType == UserType.GroupManager).Select(s => new userModel
                {
                    ID = s.ID,
                    Name = s.CyUsNm,
                }).FirstAsync();

                usersList.Add(groupMang);
            }

            if (vahedId != null)////اضافه کردن مدیر واحد به لیست چت
            {
                var vahedDep = await query.Where(x => x.CyVahedID == vahedId && x.userType == UserType.DepartmentManager).Select(s => new userModel
                {
                    ID = s.ID,
                    Name = s.CyUsNm,
                }).FirstAsync();

                usersList.Add(vahedDep);
            }


            ////اضافه کردن ایجاد کننده سفارش به لیست چت
            var creatorUser = await query.Where(x => (isPcb == false && x.ID == order.CyUserID) || (isPcb == true && x.ID == pcb.CyUserID)).
                Select(s => new userModel
                {
                    ID = s.ID,
                    Name = s.CyUsNm,
                }).
            FirstAsync();
            usersList.Add(creatorUser);



            ///اضافه کردن مدیر کل به لیست چت
            var general = await query.Where(x => x.userType == UserType.GeneralManager).Select(s => new userModel
            {
                ID = s.ID,
                Name = s.CyUsNm,
            }).FirstAsync();

            usersList.Add(general);


            ////اضافه کردن اعضای دعوت شده به لیست چت
            var userOrders = await _db.CyUserOrders.Where(x => x.OrderID == orderId).ToListAsync();
            foreach (var item in userOrders)
            {
                usersList.Add(await query.Where(x => x.ID == item.UserID).Select(s => new userModel
                {
                    ID = s.ID,
                    Name = s.CyUsNm,
                }).FirstAsync());
            }

            ////حذف اعضای تکراری در صورت وجود
            var distinctList = usersList.GroupBy(x => x.ID)
             .Select(g => g.First())
            .ToList();


            return Ok(distinctList);
        }

        class userModel
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
        }

        ////[Authorize]
        //[TypeAuthorize([UserType.SalesManager])]
        //[HttpPost("SendMessageAdmin")]
        //public async Task<IActionResult> SendMessageAdmin([FromBody] OrderMessageDTO Message)
        //{
        //    var identity = User.Identities.FirstOrDefault();
        //    if (identity == null)
        //        return Unauthorized("کاربر پیدا نشد");
        //    var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

        //    var Order = _db.CyOrder.Where(o => o.ID == Message.OrderID && o.IsVisible).FirstOrDefault();
        //    //var Customer = _db.CyUser.Where(c=> c.IsVisible && c.ID == Order.CyUserID).Include(c=> c.CyProfile).FirstOrDefault();
        //    if (Order == null)
        //        return NotFound();

        //    var NewMessage = new CyOrderMessage()
        //    {
        //        SenderID = userid,
        //        OrderID = Message.OrderID,
        //        Description = Message.Description,
        //        Status = OrderMessageStatus.Sent,
        //        SentDate = DateTime.Now,
        //        FileID = Message.FileID,
        //    };

        //    await _repo.Insert(NewMessage);
        //    Order.Notification = false;
        //    await _db.SaveChangesAsync();
        //    return Ok("پیام با موفقیت ارسال شد");
        //}

        ////[Authorize]
        //[TypeAuthorize([UserType.SalesManager])]
        //[HttpPost("sendSms")]
        //public async Task<IActionResult> sendSms(int OrderId)
        //{
        //    var identity = User.Identities.FirstOrDefault();
        //    if (identity == null)
        //        return Unauthorized("کاربر پیدا نشد");
        //    var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

        //    var Order = await _db.CyOrder.Where(o => o.IsVisible && o.ID == OrderId).FirstOrDefaultAsync();
        //    if (Order == null) return NotFound();
        //    var Customer = await _db.CyUser.Where(u => u.IsVisible && u.ID == Order.CyUserID).Include(u => u.CyProfile).FirstOrDefaultAsync();
        //    if (Customer == null) return NotFound();

        //    var CustomerName = Customer.CyProfile != null && Customer.CyProfile.Name != null ? Customer.CyProfile.Name : "مشتری";
        //    await _smsService.SendSmsAsync(Customer.CyUsNm, "hdht0efyo2k4lo8", CustomerName, OrderId);
        //    return Ok();
        //}


        //[TypeAuthorize([UserType.SalesManager])]
        ////[Authorize]
        //[HttpPost("GetMessagesByOrderIDAdmin")]
        //public async Task<IActionResult> GetMessagesByOrderIDAdmin(int OrderID)
        //{
        //    var identity = User.Identities.FirstOrDefault();
        //    if (identity == null)
        //        return Unauthorized("کاربر پیدا نشد");
        //    var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

        //    var OrderMessages = await _repo.Table.Where(m => m.IsVisible && m.OrderID == OrderID).Include(m => m.Sender.CyProfile).ToListAsync();
        //    foreach (var message in OrderMessages)
        //    {
        //        if (message.SenderID != userid && message.Status != OrderMessageStatus.Seen)
        //        {
        //            message.Status = OrderMessageStatus.Seen;
        //            message.SeenDate = DateTime.Now;
        //        }
        //    }
        //    await _db.SaveChangesAsync();
        //    var orderMessageDtos = OrderMessages.Select(m => new OrderMessageDTO
        //    {
        //        OrderID = m.OrderID,
        //        ID = m.ID,
        //        SenderID = m.SenderID,
        //        Description = m.Description,
        //        FileID = m.FileID,
        //        SeenDate = m.SeenDate,
        //        SenderName = (m.Sender != null && m.Sender.CyProfile != null) ? m.Sender.CyProfile.Name : m.Sender != null ? m.Sender.CyUsNm : null,
        //        SentDate = m.SentDate,
        //        Status = m.Status
        //    }).ToList();
        //    return Ok(orderMessageDtos);
        //}
    }
}