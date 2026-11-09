using System.Security.Claims;
using AutoMapper;
using CY_BM;
using CY_DM;
using CY_WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CY_WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CyNotificationController : ControllerBase
    {
        private readonly CyContext _db;
        private readonly IMapper _mapper;
        public CyNotificationController(CyContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        [HttpPost("addNotif")]
        async public Task<ActionResult> addNotif([FromBody] CyOrderNotificationDTO dto)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userClaim, out var userId)) return Unauthorized();

            var newNotif = _mapper.Map<CyOrderNotification>(dto);
            newNotif.CreateDate = DateTime.Now;
            newNotif.IsVisible = true;

            await _db.CyOrderNotification.AddAsync(newNotif);

            await _db.SaveChangesAsync();

            return Ok(new { msg = "اعلان ارسال شد " });

        }

        /// <summary>
        /// تمام اعلانات
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getNotif")]
        async public Task<ActionResult> getNotif(Guid? id)
        {
            var query = await _db.CyOrderNotification.Where(x => x.IsVisible).ToListAsync();

            if (id != null)
            {
                var currentNotif = query.Where(x => x.ID == id);
                if (currentNotif == null) return BadRequest("notFound");

                var dto = _mapper.Map<CyOrderNotificationDTO>(currentNotif);

                return Ok(dto);
            }

            var allNotif = _mapper.Map<List<CyOrderNotificationDTO>>(query);

            return Ok(allNotif);

        }

        /// <summary>
        /// اعلانات کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet("userNotif")]
        async public Task<ActionResult<CyOrderNotificationDTO>> userNotif()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userClaim, out var userId)) return Unauthorized();

            var userNotif = await _db.CyOrderNotification.Where(x => x.IsVisible && x.ReceiverID == userId).OrderByDescending(o => o.SentDate).ToListAsync();

            var dto = _mapper.Map<List<CyOrderNotificationDTO>>(userNotif);

            return Ok(dto);

        }


        /// <summary>
        /// تغییر وضعیت اعلان به حالت دیده شده
        /// </summary>
        /// <param name="notifId"></param>
        /// <returns></returns>
        [HttpGet("isSeenNotif")]
        async public Task<ActionResult> isSeenNotif(Guid notifId)
        {
            var currentNotif = await _db.CyOrderNotification.Where(x => x.IsVisible && x.ID == notifId).FirstOrDefaultAsync();
            if (currentNotif == null) return BadRequest("notFound");

            if (currentNotif.Status == NotificationStatus.Sent)
            {
                currentNotif.Status = NotificationStatus.Seen;
                await _db.SaveChangesAsync();
            }

            return NoContent();

        }


        /// <summary>
        /// تعداد اعلانات دیده نشده
        /// </summary>
        /// <returns></returns>
        [HttpGet("unSeenNotif")]
        async public Task<ActionResult> unSeenNotif()
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(userClaim, out Guid userId);

            var userNotifs=await _db.CyOrderNotification.Where(x=>x.IsVisible && x.ReceiverID==userId && x.Status==NotificationStatus.Sent).ToListAsync();


            return Ok(userNotifs.Count());


        }



    }
}
