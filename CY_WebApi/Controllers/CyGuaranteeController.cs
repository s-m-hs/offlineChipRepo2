//using CY_DM;
//using CY_BM;
//using CY_WebApi.DataAccess;
//using CY_WebApi.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using NuGet.Protocol.Core.Types;
//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using CY_WebApi.Services;
//using System.Security.Claims;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CyGuaranteeController : ControllerBase
//    {
//        private readonly Repository<CyGuarantee> _repository;
//        private readonly CyContext _db;
//        private readonly IMapper _mapper;

//        public CyGuaranteeController(CyContext db, IMapper mapper)
//        {
//            _repository = new Repository<CyGuarantee>(db);
//            _mapper = mapper;
//            _db = db;
//        }

//        [HttpPost]
//        public async Task<ActionResult> postNewGuarantee([FromBody] GuaranteeDTO newGuarantee)
//        {
//            var model = _mapper.Map<CyGuarantee>(newGuarantee);
//            CyUser? user = null;
//            if (newGuarantee.Phonenumber != null)
//                user = await _db.CyUser.Where(p => p.IsVisible &&
//                                                          p.CyProfile != null &&
//                                                          p.CyProfile.Mobile != null &&
//                                                          p.CyProfile.Mobile.StartsWith(newGuarantee.Phonenumber)).FirstOrDefaultAsync();
//            if (user == null)
//                return BadRequest("کاربر با این شماره تلفن پیدا نشد");
//            else
//                model.CyUserID = user.ID;
//            try
//            {
//                await _repository.Insert(model);
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return Ok("با موفقیت اضافه شد");
//        }

//        [HttpGet]
//        public async Task<ActionResult<List<GuaranteeDTO>>> getGuarantees(string phoneNumber)
//        {
//            var result = await _repository.TableNoTracking.Where(g => g.IsVisible && g.CyUser.CyProfile.Mobile.StartsWith(phoneNumber))
//                .Select(g => new GuaranteeDTO()
//                {
//                    ID = g.ID,
//                    CompanyExplaination = g.CompanyExplaination,
//                    Details = g.Details,
//                    GuaranteeCompany = g.GuaranteeCompany,
//                    GuaranteeID = g.GuaranteeID,
//                    GuarantreePrice = g.GuarantreePrice,
//                    Phonenumber = g.CyUser.CyProfile.Mobile,
//                    ProductName = g.ProductName,
//                    ProductProblem = g.ProductProblem,
//                    ProductStatus = g.ProductStatus,
//                    RecievedDate = g.RecievedDate.Value,
//                    Username = g.CyUser.CyUsNm
//                }).ToListAsync();
//            return Ok(result);
//        }

//        [HttpGet("getAll")]
//        public async Task<ActionResult<PageResultDTO<GuaranteeDTO>>> getAllGuarantees(int page, int size)
//        {
//            var skipCount = page * size;
//            var result = await _repository.TableNoTracking.Where(g=> g.IsVisible &&
//                                                                 g.RecievedDate != null).Skip(skipCount).Take(size).Select(g=> new GuaranteeDTO()
//            {
//                ID = g.ID,
//                CompanyExplaination = g.CompanyExplaination,
//                Details = g.Details,
//                GuaranteeCompany = g.GuaranteeCompany,
//                GuaranteeID = g.GuaranteeID,
//                GuarantreePrice = g.GuarantreePrice,
//                Phonenumber = g.CyUser.CyProfile.Mobile,
//                ProductName = g.ProductName,
//                ProductProblem = g.ProductProblem,
//                ProductStatus = g.ProductStatus,
//                RecievedDate = g.RecievedDate.Value,
//                Username  = g.CyUser.CyUsNm,
//                Type = g.Type
//            }).ToListAsync();
//            var allCount = await _repository.TableNoTracking.Where(g=> g.IsVisible && g.RecievedDate != null).CountAsync();
//            return Ok(new PageResultDTO<GuaranteeDTO>
//            {
//                ItemList = result,
//                AllCount = allCount
//            });
//        }

//        [HttpDelete("deleteGuarantee")]
//        public async Task<ActionResult<CyGuarantee>> deleteGuaranteeById(int id)
//        {
//            var guarantee = await _db.CyGuarantee.Where(g=> g.IsVisible && g.ID == id).FirstOrDefaultAsync();
//            if (guarantee == null) { return BadRequest("گارانتی با این شناسه یافت نشد"); }
//            guarantee.IsVisible = false;
//            guarantee.LastModified = DateTime.Now;
//            _db.CyGuarantee.Update(guarantee);
//            await _db.SaveChangesAsync();
//            return Ok(new {msg = "گارانتی حذف شد"});
//        }

//        [HttpPut("updateGuarantee")]
//        public async Task<ActionResult<CyGuarantee>> updateGuartee(UpdateGuaranteeDTO newValues)
//        {
//            var updatedGuarantee = await _repository.Table.Where(g=>g.IsVisible && g.ID == newValues.ID).FirstOrDefaultAsync();
//            if(updatedGuarantee == null) return BadRequest("not found");

//            updatedGuarantee.ProductName = newValues.ProductName;
//            updatedGuarantee.ProductProblem = newValues.ProductProblem;
//            updatedGuarantee.Details = newValues.Details;
//            updatedGuarantee.ProductStatus = newValues.ProductStatus;
//            updatedGuarantee.GuaranteeCompany = newValues.GuaranteeCompany;
//            updatedGuarantee.CompanyExplaination = newValues.CompanyExplaination;
//            updatedGuarantee.GuarantreePrice = newValues.GuarantreePrice;
//            updatedGuarantee.LastModified = DateTime.Now;

//            _db.Update(updatedGuarantee);
//            await _db.SaveChangesAsync();
//            return Ok("بروزرسانی شد");
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("getForUser")]
//        public async Task<ActionResult<GuaranteeDTO>> getUserGuarantee()
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var result = await _repository.TableNoTracking.Where(g=> g.IsVisible && g.CyUserID ==  userid).ToListAsync();
//            return Ok(_mapper.Map<List<GuaranteeDTO>>(result));
//        }
//    }
//}