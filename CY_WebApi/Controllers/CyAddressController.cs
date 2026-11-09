//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using CY_DM;
//using CY_BM;
//using CY_WebApi.Services;
//using CY_WebApi.Models;
//using System.Runtime.InteropServices;
//using AutoMapper;
//using CY_WebApi.DataAccess;
//using System.Security.Claims;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.OpenApi.Validations;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CyAddressController : ControllerBase
//    {
//        private readonly IMapper _mapper;
//        private readonly Repository<CyAddress> _repo;
//        private readonly CyContext _db;

//        public CyAddressController(CyContext context, IMapper mapper)
//        {
//            _mapper = mapper;
//            _repo = new Repository<CyAddress>(context);
//            _db = context;

//        }

//        // GET: api/CyAddress
//        [TypeAuthorize([UserType.Customer])]
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAddress()
//        {
//            //var un = User.Identity.Name;
//            //string idAdress = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
//            var d=User.Identities.FirstOrDefault();
//            if (d != null)
//            {
//                var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
//                var resu = await _repo.TableNoTracking.Where(a=>a.CyUserID == Int32.Parse(userid)).ToListAsync();
//                return Ok(resu.Select(c => _mapper.Map<AddressDTO>(c)).ToList());
//            }
//           return NoContent();
//        }

//        // GET: api/CyAddress/5
//        //[TypeAuthorize([UserType.Customer])]
//        //[HttpGet("{id}")]
//        //public async Task<ActionResult<AddressDTO>> GetAddress(int id)
//        //{
//        //    var un = User.Identity.Name;
//        //    var user = await _db.CyUser.Where(c => c.IsVisible && c.CyUsNm == un).FirstOrDefaultAsync();

//        //    var CyAddress = await _repo.Find(id);
//        //    if (CyAddress == null)
//        //        return NotFound();
            
//        //    if (user.ID != CyAddress.CyUserID)
//        //        return Unauthorized("user can not access this address");

//        //    return _mapper.Map<AddressDTO>(CyAddress);
//        //}

//        // PUT: api/CyAddress/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [TypeAuthorize([UserType.Customer])]
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutAddress(int id, AddressPutDTO AddressDTO)
//        {
//            var d = User.Identities.FirstOrDefault();
//            if(d != null)
//            {
//                var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
               
//                var newAddress = _mapper.Map<CyAddress>(AddressDTO);
//                newAddress.ID = id;
//                try
//                {
//                    if (newAddress.CyUserID == Int32.Parse(userid))
//                       return Ok(await _repo.Update(newAddress));
//                    else
//                        return Unauthorized("user can not access this address");
//                }
//                catch (Exception ex)
//                {
//                    return BadRequest(ex.Message);
//                }
//            }
//            else
//            {
//                return Unauthorized(); 
//            }
//        }

//        // POST: api/CyAddress
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [TypeAuthorize([UserType.Customer])]
//        [HttpPost("PostAddress")]
//        public async Task<ActionResult<AddressDTO>> PostAddress(AddressDTO AddressDTO)
//        {
//            var d = User.Identities.FirstOrDefault();

//            if (d != null)
//            {
//                var userid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
//                var CyAddress = _mapper.Map<CyAddress>(AddressDTO);
//                CyAddress.CyUserID = Int32.Parse(userid);
//                CyAddress = await _repo.Insert(CyAddress);
//                return CreatedAtAction(nameof(GetAddress), new { id = CyAddress.ID }, _mapper.Map<AddressDTO>(CyAddress));
//            }

//            return BadRequest();
//        }


//        // DELETE: api/CyAddress/5
//        [TypeAuthorize([UserType.Customer])]
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteAddress(int id)
//        {
//            var un = User.Identity.Name;
//            var user = await _db.CyUser.Where(c => c.IsVisible && c.CyUsNm == un).FirstOrDefaultAsync();

//            var Address = await _repo.Find(id);

//            if(Address.CyUserID == user.ID)
//            {
//                await _repo.Delete(id);
//                return NoContent();
//            }

//            return Unauthorized("user can not access this address");
//        }
//    }
//}