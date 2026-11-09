using AutoMapper;
using CY_BM;
using CY_DM;
using CY_WebApi.Models;
using CY_WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace CY_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CyLoginController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly CyContext _db;

        public CyLoginController(JwtService jwtService, CyContext db, IMapper mapper)
        {
            _jwtService = jwtService;
            _db = db;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginModel>> Login([FromBody] LoginModel login)
        {
            // Replace with your actual user validation logic (don't store plain text passwords)
            CyUser? user = await _jwtService.IsValidUser(login, _db);
            if (user == null)
            {
                return Unauthorized();
            }

            //var user = GetUserFromCredentials(login); // Replace with logic to get user object

            var token = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken(user);

            _jwtService.SetTokenInCookie(token, HttpContext, "Admin");
            _jwtService.SetRefreshTokenInCookie(refreshToken, HttpContext, "Admin");
            return Ok(new { token = token, type = user.userType });

        }
       
        
        
        [HttpGet("getUserDetile")]
        public async Task<ActionResult<UserSelect>> getUserDetile()
        {
            var userClaim=User.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.NameIdentifier)?.Value;
            var userClaimB=User.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.Role)?.Value;
            if(!Guid.TryParse(userClaim, out Guid userId) || userClaimB==null ) return Unauthorized();

            var curentUser=await _db.CyUser.
                Where(x=>x.IsVisible && x.ID == userId).Include(i => i.CyVahed).Include(i => i.CyGorooh).Select(s=>new UserSelect
                {
                    id = s.ID,
                    userName = s.CyUsNm,
                    role = userClaimB,
                    vahedText = s.CyVahed != null ? s.CyVahed.Text : null,
                    group = s.CyGorooh != null ? s.CyGorooh.Text : null,
                    vahedId = s.CyVahedID,
                    gruopId = s.CyGoroohID,
                })
                //.Select(s => new
                //{
                //    id = s.ID,
                //    userName = s.CyUsNm,
                //    role = userClaimB,
                //    vahedText = s.CyVahed != null ? s.CyVahed.Text : null,
                //    group = s.CyGorooh != null ? s.CyGorooh.Text : null,
                //    vahedId = s.CyVahedID,
                //    gruopId = s.CyGoroohID,
                //})
                .FirstOrDefaultAsync();


            return Ok(curentUser);
        }



        [HttpGet("logout")]
        public IActionResult Logout()
        {
            string subdomain = HttpContext.Request.Headers.Origin.ToString();
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Domain = DomainConfig.Domain
            };

            Response.Cookies.Delete("AdminAccessTokenOffLine", options);
            Response.Cookies.Delete("AdminRefreshTokenOffLine", options);

            return Ok(new { msg = "Logged out successfully" });
        }


        [HttpGet("refreshToken")]
        public IActionResult refreshToken()
        {
            if (!Request.Cookies.TryGetValue("AdminRefreshTokenOffLine", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("Refresh token not found");
            }

            var principal = _jwtService.GetPrincipalFromExpiredToken(refreshToken);
            if (principal == null)
            {
                return Unauthorized("Invalid refresh token");
            }
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID claim not found");
            }
            Guid userid = Guid.Parse(userIdClaim.Value);
            var user = _db.CyUser.Where(u => u.IsVisible && u.ID == userid).FirstOrDefault();
            if (user == null) { return BadRequest(); }

            // Generate new tokens
            var newAccessToken = _jwtService.GenerateToken(user);

            //remove old token:
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Domain = DomainConfig.Domain
            };
            Response.Cookies.Delete("AdminAccessTokenOffLine", options);

            // Set new token in cookies
            _jwtService.SetTokenInCookie(newAccessToken, HttpContext, "Admin");

            return Ok(new { token = newAccessToken });
        }

        [HttpPost("Hash")]
        public async Task<IActionResult> HashTest([FromBody] LoginModel login)
        {
            // Replace with your actual user validation logic (don't store plain text passwords)
            return Ok(new { hash = Crypto.GetStringSha512Hash(login.Pw) });
        }


    }
}
