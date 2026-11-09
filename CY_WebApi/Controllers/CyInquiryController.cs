using System.Security.Claims;
using AutoMapper;
using CY_BM;
using CY_DM;
using CY_WebApi.Migrations;
using CY_WebApi.Models;
using CY_WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyInquiryController : ControllerBase
    {
        private readonly CyContext _db;
        private readonly IMapper _mapper;

        public CyInquiryController(CyContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;


        }


        [HttpPost("addInquiry")]
        public async Task<ActionResult> addInquiry([FromBody] InquiryOrderDTO dto)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized();

            if (dto.RequestFile == null) return BadRequest(new { msg = "فایلی ارسال نشده است" });

            var currentUser = await _db.CyUser.Where(x => x.IsVisible && x.ID == userId).Select(s => s.CyUsNm).FirstOrDefaultAsync();

            var newInq = _mapper.Map<CyInquiryOrder>(dto);

            newInq.CyUserID = userId;
            newInq.CreatorName = currentUser;

            await _db.CyInquiryOrder.AddAsync(newInq);
            await _db.SaveChangesAsync();

            return Ok(new { msg = "درخواست با موفقیت ارسال شد" });
        }



        /// <summary>
        /// همه استعلام ها
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getInquiry")]
        public async Task<ActionResult<InquiryOrderDTO>> getinquiry(Guid? id)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized();

            UserType userType = UserTypeExtensionsB.GetRoleType(userClaimRole);


            var query = _db.CyInquiryOrder.Where(x => x.IsVisible).Include(i => i.CyUser).AsQueryable();

            if (id != null)
            {
                var currentInqu = query.Where(x => x.ID == id).FirstOrDefault();

                return Ok(currentInqu);
            }


            if (userType == UserType.GroupExpert || userType == UserType.GroupManager || userType == UserType.DepartmentManager)
            {


                var inquS=await query.Where(x=>x.CyUserID == userId).ToListAsync();
                var dto = _mapper.Map<List<InquiryOrderDTO>>(inquS);


                return Ok(dto);

            }


            var allInqu =await query.ToListAsync();

            var dtoList = _mapper.Map<List<InquiryOrderDTO>>(allInqu);

            return Ok(dtoList);

        }

        /// <summary>
        /// استعلام های باز
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getRequestedInquiry")]
        public async Task<ActionResult <InquiryOrderDTO>> getRequestedInquiry(Guid? id)

        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized();

            UserType userType = UserTypeExtensionsB.GetRoleType(userClaimRole);


            var allInqu =await _db.CyInquiryOrder.Where(x => x.IsVisible && x.ResponseFile==null).Include(i => i.CyUser).ToListAsync();



            var dtoList = _mapper.Map<List<InquiryOrderDTO>>(allInqu);



            return Ok(dtoList);

        }



        /// <summary>
        /// پاسخ به استعلام توسط فروشنده
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("editInquiry")]
        public async Task<ActionResult> editInquiry([FromBody] InquiryOrderDTO dto)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userClaimRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userClaim, out Guid userId)) return Unauthorized();

            UserType userType = UserTypeExtensionsB.GetRoleType(userClaimRole);

            if (userType != UserType.PurchasingExpert && userType != UserType.PurchasingManager) return BadRequest(new { msg = "شما مجاز به تغییرات نیستید" });

            if (dto.ID == null) return BadRequest(new { msg = "" });

            var currentInqu=await _db.CyInquiryOrder.Where(x=>x.IsVisible && x.ID==dto.ID).FirstOrDefaultAsync();

            if (currentInqu == null) return BadRequest(new {msg="استعلام پیدا نشد"});

            currentInqu.CommentB=dto.CommentB;
            currentInqu.ResponseFile=dto.ResponseFile;
            currentInqu.ResponseDate=DateTime.Now;


           await _db.SaveChangesAsync();

            return Ok(new {msg="استعلام با موفقیت ارسال شد"});

        }


    }
}
