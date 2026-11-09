using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CY_DM;
using CY_BM;
using CY_WebApi.Models;
using System.Runtime.InteropServices;
using AutoMapper;
using CY_WebApi.DataAccess;
using CY_WebApi.Services;

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CyUsersController : ControllerBase
    {
        //private readonly Repository<CyUser> _repo;
        private readonly CyContext _db;
        private readonly IMapper _mapper;

        public CyUsersController(CyContext context, IMapper mapper, CyContext db )
        {
            _mapper = mapper;
            _db = db;

            //_repo = new Repository<CyUser>(context);
        }

        [Route("GetUserByType/{type}")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<UserSelectB>>> GetUserByType(UserType? type)
        {
            var resu = new List<CyUser>();
            var profiles = new List<ProfileDTO>();
            var query = _db.CyUser.Where(x => x.IsVisible).Include(i=>i.CyGorooh).Select(s => new UserSelectB
            {
                id=s.ID,
                userName = s.CyUsNm,
                Access = s.AccessTable.Text,
                AccessID=s.AccessTableID,
                Group = s.CyGorooh.Text,
                GroupID = s.CyGorooh.ID,
                Vahed = s.CyVahed.Text,
                VahedID = s.CyVahed.ID,
                UserType=s.userType,
                UserStatus=s.Status
            }).ToList();

            if (type == 0)
            {
                return Ok(query);
            }

            var users = query.Where(x => x.UserType == type).ToList();  

            return Ok(users);

        }


        private UserDTO removPass(UserDTO userDTO)
        {
            userDTO.CyHsPs = "***";
            return userDTO;
        }




        // GET: api/CyUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileDTO>> GetCyUser(Guid id)
        {
            var cyUser = await _db.CyUser.Where(c => c.IsVisible && c.ID == id).Include(c => c.CyProfile).FirstOrDefaultAsync();
            if (cyUser == null) return NotFound();
            if (cyUser.CyProfile == null)
            {
                if (cyUser == null)
                {
                    return NotFound();
                }
                cyUser.CyHsPs = "***";
                return Ok(_mapper.Map<UserDTO>(cyUser));
            }
            var profileDTO = _mapper.Map<ProfileDTO>(cyUser.CyProfile);
            profileDTO.Username = cyUser.CyUsNm;
            profileDTO.CyUserID = cyUser.ID;
            return Ok(profileDTO);
        }

        // PUT: api/CyUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCyUser( UserDTO userDTO)
        {
            var toChange = _db.CyUser.Where(c => c.IsVisible && c.ID == userDTO.ID).FirstOrDefault();
            if (toChange != null)
            {
                toChange.CyUsNm = userDTO.CyUsNm;
                toChange.CyHsPs = userDTO.CyHsPs;
                toChange.Status = userDTO.Status;
                toChange.userType = userDTO.userType;
                toChange.AccessTableID = userDTO.AccessTableID;
                toChange.CyVahedID = userDTO.CyVahedID;
                toChange.CyGoroohID = userDTO.CyGoroohID;
                if (userDTO.ID != toChange.ID)
                    return BadRequest();

                await _db.SaveChangesAsync();
                return Ok(new {msg=""});
            }
            else
                return BadRequest();
        }

        // POST: api/CyUsers
        //[TypeAuthorize([UserType.SysAdmin])]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostCyUser(UserDTO UserDto)
        {

            var cyUser = _db.CyUser.Where(c => c.CyUsNm == UserDto.CyUsNm && c.IsVisible).FirstOrDefault();
            if (cyUser != null)
            {
                return BadRequest("این نام کاربری قبلا استفاده شده است");
            }

            else
            {
                if (UserDto.AccessTableID == null) return BadRequest(new { msg = "سطح دسترسی تعیین نشده است" });

                cyUser = _mapper.Map<CyUser>(UserDto);


                cyUser.CyHsPs = Crypto.EncryptStringAES(UserDto.CyHsPs);

                await _db.CyUser.AddAsync(cyUser);
                await _db.SaveChangesAsync();
                return CreatedAtAction("GetCyUser", new { id = cyUser.ID }, removPass(_mapper.Map<UserDTO>(cyUser)));

            }
        }

        // DELETE: api/CyUsers/5
        [HttpDelete("{id}")]
        [TypeAuthorize([UserType.SysAdmin,UserType.GeneralManager,UserType.CustomerAdmin])]
        public async Task<IActionResult> DeleteCyUser(Guid id)
        {
            var user = await _db.CyUser.Where(x => x.IsVisible && x.ID == id).FirstOrDefaultAsync();
            if (user == null) return BadRequest("user notfound");

            user.IsVisible = false;
            await _db.SaveChangesAsync();

            return Ok();
        }




    }
}
