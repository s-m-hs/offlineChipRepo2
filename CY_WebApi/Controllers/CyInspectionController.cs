//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using CY_WebApi.Services;
//using CY_BM;
//using NuGet.Protocol.Core.Types;
//using CY_DM;
//using CY_WebApi.Models;
//using CY_WebApi.DataAccess;
//using System.Security.Claims;
//using Microsoft.EntityFrameworkCore;
//using AutoMapper;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CyInspectionController : ControllerBase
//    {
//        private readonly IMapper _mapper;
//        private readonly CyContext _db;
//        private readonly Repository<CyInspectionForm> _repo;

//        public CyInspectionController(CyContext db, IMapper mapper)
//        {
//            _mapper = mapper;
//            _db = db;
//            _repo = new Repository<CyInspectionForm>(db);
//        }

//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet("getInspectionsForCustomer")]
//        public async Task<ActionResult<List<CyInspectionForm>>> getInspectionsForCustomer()
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var userInspections = await _repo.TableNoTracking.Where(i => i.UserID == userid && i.IsVisible)
//                                                   .Include(i => i.Items.Where(i => i.IsVisible)).ToListAsync();
//            return Ok(_mapper.Map<List<InspectionFormDTO>>(userInspections));
//        }

//        //[TypeAuthorize([UserType.SysAdmin])]
//        [HttpPost("getAllInspection")]
//        public async Task<ActionResult<List<CyInspectionForm>>> getAllInspection(PagedOrdersDto pages)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var skipCount = pages.PageNumber * pages.PageSize;

//            var AllCount = await _repo.TableNoTracking.Where(i => i.IsVisible).CountAsync();

//            var userInspections = await _repo.TableNoTracking.Where(i => i.IsVisible)
//                                                        // .Include(i => i.Items)
//                                                         .Skip(skipCount).Take(pages.PageSize)
//                                                         .OrderByDescending(o => o.CreateDate)
//                                                         .ToListAsync();

//            var result = new PageResultDTO<InspectionFormDTO>
//            {
//                ItemList = _mapper.Map<List<InspectionFormDTO>>(userInspections),
//                AllCount = AllCount,
//            };
//            return Ok(result);
//        }

//        //[TypeAuthorize([UserType.SysAdmin])]
//        [HttpGet("getInsceptionItems/{InspectionID}")]
//        public async Task<IActionResult> getInspectionItems(int InspectionID)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");

//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var items = await _repo.TableNoTracking.Where(i=> i.IsVisible &&
//                                                 i.ID == InspectionID)
//                                                 .Select(i=> i.Items).ToListAsync();
//            var result = _mapper.Map<List<InspectionItemDTO>>(items);
//            return Ok(result);
//        }

//        //[TypeAuthorize([UserType.Customer])]
//        //[HttpPost("postInspection")]
//        //public async Task<ActionResult<List<CyInspectionForm>>> postInspection([FromBody] InspectionFormDTO input)
//        //{
//        //    var identity = User.Identities.FirstOrDefault();
//        //    if (identity == null)
//        //        return Unauthorized("کاربر پیدا نشد");

//        //    var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//        //    if (input.Items == null || input.Items.Count < 1)
//        //        return BadRequest();

//        //    var newInspection = _mapper.Map<CyInspectionForm>(input);
//        //    newInspection.Items = _mapper.Map<List<CyInspectionItem>>(input.Items);
//        //    newInspection.UserID = userid;

//        //    if(newInspection.Lab == InspectionLab.Iran)
//        //    {
//        //        newInspection.PinCorrelationTest = false;
//        //        newInspection.XRFTest = false;
//        //        newInspection.KeyFunctional = false;
//        //        newInspection.Baking = false;
//        //        newInspection.TapeAndReel = false;
//        //        newInspection.InternalVisualInspection = false;
//        //    }
            
//        //    var inserted = await _repo.Insert(newInspection);
//        //    foreach (var item in inserted.Items)
//        //    {
//        //        item.CyInspectionFormID = inserted.ID;
//        //    }
//        //    int i = await _repo.SaveChangesAsync();
//        //    return Ok(i);
//        //}

//        //[TypeAuthorize()]

//    }
//}