//using CY_BM;
//using CY_DM;
//using CY_WebApi.DataAccess;
//using CY_WebApi.Models;
//using CY_WebApi.Services;
//using Google.Cloud.RecaptchaEnterprise.V1;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using NuGet.Protocol.Core.Types;
//using System.Diagnostics.Contracts;
//using System.Security.Claims;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CyTicketController : ControllerBase
//    {
//        private readonly Repository<CyTicket> _repo;
//        private readonly Repository<CyOrderMessage> _messageRepo;
//        private readonly SmsService _smsService;
//        private readonly CyContext _db;

//        public CyTicketController(CyContext db, SmsService smsService)
//        {
//            _db = db;
//            _repo = new Repository<CyTicket>(db);
//            _messageRepo = new Repository<CyOrderMessage>(db);
//            _smsService = smsService;
//        }


//        [HttpPost("createTicket")]
//        public async Task<ActionResult> CreateTicket([FromBody] TicketDTO newTicket)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            var userid = 0;
//            if (identity != null && identity.IsAuthenticated)
//                userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var ticket = new CyTicket()
//            {
//                OpenedAt = DateTime.Now,
//                Title = newTicket.Title,
//                Topic = newTicket.Topic,
//                PhoneNumber = newTicket.PhoneNumber,
//                Status = TicketStatus.Waiting,
//                Messages = new List<CyOrderMessage>(),
//                UserId = userid != 0 ? userid : null,
//            };
//            ticket = await _repo.Insert(ticket);
//            return Ok(ticket);
//        }

        
//        [HttpPost("postOnTicket")]
//        public async Task<ActionResult> postOnTicket([FromBody] TicketMessageDTO Message)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            var userid = 0;
//            if (identity != null && identity.IsAuthenticated)
//                userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var Ticket = _repo.Table.Where(t => t.ID == Message.TicketID &&
//                                              t.IsVisible && ( (userid == 0 && t.UserId == null) || t.UserId == userid) && t.Status != TicketStatus.Closed).FirstOrDefault();

//            if (Ticket == null)
//                return NotFound();

//            var NewMessage = new CyOrderMessage()
//            {
//                SenderID = userid != 0 ? userid : null,
//                TicketID = Ticket.ID,
//                Description = Message.Description,
//                Status = OrderMessageStatus.Sent,
//                SentDate = DateTime.Now,
//                FileID = Message.FileID,
//            };
//            Ticket.Status = TicketStatus.Waiting;
//            await _messageRepo.Insert(NewMessage);
//            return Ok("پیام با موفقیت ارسال شد");
//        }

//        //forAdmin
//        [HttpPost("postOnTicketAdmin")]
//        [TypeAuthorize([UserType.SalesManager])]
//        public async Task <ActionResult> postOnTicketAdmin([FromBody] TicketMessageDTO Message)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            var userid = 0;
//            if (identity != null && identity.IsAuthenticated)
//                userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var Ticket = _repo.Table.Where(t => t.ID == Message.TicketID &&
//                                           t.IsVisible && t.Status != TicketStatus.Closed).FirstOrDefault();

//            if (Ticket == null)
//                return NotFound();

//            var NewMessage = new CyOrderMessage()
//            {
//                SenderID = userid,
//                TicketID = Ticket.ID,
//                Description = Message.Description,
//                Status = OrderMessageStatus.Sent,
//                SentDate = DateTime.Now,
//                FileID = Message.FileID,
//            };
//            await _smsService.SendSmsAsync(NewMessage.Description, "hdht0efyo2k4lo8", Message.SenderName, Message.ID);
//            Ticket.Status = TicketStatus.Answered;
//            await _messageRepo.Insert(NewMessage);
//            return Ok("پیام با موفقیت ارسال شد");
//        }


//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("getUserTickets")]
//        public async Task<ActionResult> getUserTickets()
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var tickets = await _repo.TableNoTracking.Where(t => t.IsVisible && t.UserId == userid).Select(t => new TicketDTO()
//            {
//                ID = t.ID,
//                OpenedAt = t.OpenedAt,
//                ClosedAt = t.ClosedAt,
//                Status = t.Status,
//                Title = t.Title,
//                Topic = t.Topic,
//                UserId = userid
//            }).ToListAsync();

//            return Ok(tickets);
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("getMessagesForTicketUser")]
//        public async Task<ActionResult> getMessagesForUser(int TicketID)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var userName = await _db.CyUser.Where(u=> u.ID == userid).Select(u=> u.CyProfile != null ? u.CyProfile.Name : u.CyUsNm).FirstOrDefaultAsync();

//            var messages = await _messageRepo.TableNoTracking.Where(m => m.TicketID == TicketID && m.Ticket != null && m.Ticket.UserId == userid)
//                                                       .Select(m => new TicketMessageDTO()
//                                                       {
//                                                           ID = m.ID,
//                                                           TicketID = m.TicketID,
//                                                           Description = m.Description,
//                                                           FileID = m.FileID,
//                                                           SeenDate = (m.SeenDate == null && m.SenderID != userid) ? DateTime.Now : m.SeenDate,
//                                                           SentDate = m.SentDate,
//                                                           SenderName = (m.SenderID == userid) ? userName : "چیپ یاب",
//                                                           SenderID = m.SenderID,
//                                                           Status = m.Status,
//                                                       }).ToListAsync();
//            foreach (var message in messages)
//            {
//                if(message.SeenDate == null && message.SenderID != userid)
//                {
//                    var toUpdate = await _messageRepo.Table.Where(m => m.ID == message.ID).FirstOrDefaultAsync();
//                    if(toUpdate != null)
//                        toUpdate.SeenDate = message.SeenDate;
//                }
//            }
//            await _db.SaveChangesAsync();
//            return Ok(messages);
//        }


//        [HttpGet("getAllTickets")]
//        [TypeAuthorize([UserType.SalesManager, UserType.Reporter])]
//        public async Task<ActionResult> getAllTickets(TicketStatus status)
//        {
//            var tickets = await _repo.TableNoTracking.Where(t => t.IsVisible && t.Status == status).Select(t => new TicketDTO()
//            {
//                OpenedAt = t.OpenedAt,
//                ClosedAt = t.ClosedAt,
//                ID = t.ID,
//                Title = t.Title,
//                Status = t.Status,
//                Topic = t.Topic,
//                UserId = t.UserId != null ? (int)t.UserId : null,
//            }).ToListAsync();

//            return Ok(tickets);
//        }


//        [HttpPut("closeTicket")]
//        [TypeAuthorize([UserType.SalesManager])]
//        public async Task<ActionResult> closeTicket(int ticketId)
//        {
//            var ticket = await _repo.Table.Where(t=> t.IsVisible && t.ID == ticketId).FirstOrDefaultAsync();
//            if (ticket == null) return BadRequest();

//            ticket.Status = TicketStatus.Closed;
//            ticket.ClosedAt = DateTime.Now;
//            await _repo.Update(ticket);
//            return Ok("ticket closed");
//        }

//        [HttpGet("getMessagesForTickets")]
//        [TypeAuthorize([UserType.SalesManager])]
//        public async Task<ActionResult> getMessagesForTicket(int TicketID)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            var userid = 0;
//            if (identity != null && identity.IsAuthenticated)
//                userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var ticket = await _db.CyTicket.Where(t=> t.IsVisible && t.ID == TicketID).FirstOrDefaultAsync();
//            if (ticket == null) return BadRequest();
//            var senderPhone = ticket.PhoneNumber;

//            var messages = await _messageRepo.TableNoTracking.Where(m => m.TicketID == TicketID && m.Ticket != null)
//                                           .Select(m => new TicketMessageDTO()
//                                           {
//                                               ID = m.ID,
//                                               TicketID = m.TicketID,
//                                               Description = m.Description,
//                                               FileID = m.FileID,
//                                               SeenDate = (m.SeenDate == null && m.SenderID != userid) ? DateTime.Now : m.SeenDate,
//                                               SentDate = m.SentDate,
//                                               SenderName = m.SenderID == userid ? null : m.Sender != null ? m.Sender.CyProfile != null ? m.Sender.CyProfile.Name : m.Sender.CyUsNm : senderPhone,
//                                               SenderID = m.SenderID,
//                                               Status = m.Status,
//                                           }).ToListAsync();
//            foreach (var message in messages)
//            {
//                if (message.SeenDate == null && message.SenderID != userid)
//                {
//                    var toUpdate = await _messageRepo.Table.Where(m => m.ID == message.ID).FirstOrDefaultAsync();
//                    if (toUpdate != null)
//                        toUpdate.SeenDate = message.SeenDate;
//                }
//            }
//            await _db.SaveChangesAsync();
//            return Ok(messages);
//        }
//    }
//}
