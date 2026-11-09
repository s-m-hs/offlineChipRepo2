//using AutoMapper;
//using CY_BM;
//using CY_DM;
//using CY_WebApi.Models;
//using CY_WebApi.Services;
//using Google.Api;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
//using Newtonsoft.Json.Linq;
//using System.Security.Claims;

//namespace CY_WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CustomerController : ControllerBase
//    {
//        private readonly JwtService _jwtService;
//        private readonly RecaptchaService _recaptchaService;
//        private readonly CyContext _db;
//        private readonly IMapper _mapper;
//        private readonly DataAccess.Repository<CyProfile> _repo;
//        private readonly SmsService _smsService;
//        private readonly Random _random;

//        public CustomerController(JwtService jwtService, CyContext db, IMapper mapper, RecaptchaService recaptchaService, SmsService smsService)
//        {
//            _jwtService = jwtService;
//            _db = db;
//            _mapper = mapper;
//            _repo = new DataAccess.Repository<CyProfile>(_db);
//            _recaptchaService = recaptchaService;
//            _smsService = smsService;
//            _random = new Random();
//        }
        
//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginModel login)
//        {
//            // Replace with your actual user validation logic (don't store plain text passwords)
//            CyUser? user = await _jwtService.IsValidUser(login, _db);
//            if (user == null)
//            {
//                return Unauthorized();
//            }
//            else
//            {
//                var refreshToken = _jwtService.GenerateRefreshToken(user);
//                var token = _jwtService.GenerateToken(user);

//                _jwtService.SetRefreshTokenInCookie(refreshToken, HttpContext);
//                _jwtService.SetTokenInCookie(token, HttpContext);
//                return Ok(new { token = token });

//            }
//        }
//        [HttpPost("verify")]
//        public async Task<IActionResult> VerifyRecaptcha([FromBody] RecaptchaRequest request)
//        {
//            var response = await _recaptchaService.VerifyCaptcha(request.Token);

//            if (response)
//            {
//                return Ok(response);
//            }
//            else
//            {
//                return BadRequest(response);
//            }
//        }

//        [HttpPost("register")]
//        public async Task<IActionResult> Register([FromBody] RegisterModel register)
//        {
//            register.Un = register.Un.Trim();
//            var convertedUsername = _smsService.ConvertPhoneNumber(register.Un);
//            if (convertedUsername == null)
//                return BadRequest(new { msg = "شماره همراه معتبر نمی باشد" });
//            var usr = _db.CyUser.Where(c => c.IsVisible && ( c.CyUsNm == register.Un || c.CyUsNm == convertedUsername)).FirstOrDefault();

//            if (usr != null && usr.Status == UserStatus.Active)
//            {
//                return BadRequest(new { msg = "این نام کاربری موجود می باشد" });
//            }
//            else if(usr != null)
//            {
//                usr.IsVisible = false;
//                await _db.SaveChangesAsync();
//            }
//            var profile = new CyProfile()
//            {
//                Name = register.Name,
//                Mobile = register.Un,
//                CreateDate = DateTime.Now,
//                IsVisible = true,

//            };

//            //FOR REGISTERING VIA SMS
//            var validationCode = _random.Next(100000, 1000000);

//            usr = new CyUser()
//            {
//                CreateDate = DateTime.Now,
//                CyHsPs = Crypto.EncryptStringAES(register.Pw),
//                CyUsNm = register.Un,
//                IsVisible = true,
//                LastLoginTime = DateTime.Now,
//                Status = UserStatus.UnConfirmed,
//                userType = UserType.Customer,
//                MaxWrongPass = 10,
//                WrongPass = 0,
//                CyProfile = profile,

//                //FOR REGISTERING VIA SMS
//                RegisterCode = validationCode,
//                RegisterCodeTimer = DateTime.Now.AddMinutes(5),
//            };

//            //register.Pw = Crypto.GetStringSha512Hash(register.Pw);
//            //CyUser? user = await _jwtService.IsValidUser(new LoginModel() { Pw = register.Pw, Un = register.Un }, _db);
//            //if (user == null)
//            //{
//            //    return Unauthorized();
//            //}
//            //else
//            //{
//            //    var token = _jwtService.GenerateToken(user);
//            //    return Ok(new { token = token });
//            //}

//            //FOR REGISTERING VIA SMS
//            try
//            {
//                await _smsService.SendRegisterSmsAsync(register.Un, validationCode);
//            }
//            catch (Exception ex)
//            {
//                //await _smsService.SendRegisterSmsAsync(register.Un, validationCode);
//            }
//            await _db.CyUser.AddAsync(usr);
//            await _db.SaveChangesAsync();
//            return Ok(new { msg = "کد تایید برای شما ارسال شد" });

//        }

//        [HttpPost("verifyCode")]
//        public async Task<ActionResult> verifyCode(ValidatePasswordDTO info)
//        {
//            var convertedUsername = _smsService.ConvertPhoneNumber(info.Username);
//            if (convertedUsername == null) return BadRequest("شماره همراه معتبر نمی باشد");

//            var user = _db.CyUser.Where(u => u.IsVisible && ( u.CyUsNm == info.Username || u.CyUsNm == convertedUsername)).FirstOrDefault();
//            if(user == null) return BadRequest("نام کاربری پیدا نشد");


//            var validationCode = user.RegisterCode;
            
//            if (validationCode == null || user.RegisterCodeTimer <= DateTime.Now)
//            {
//                return BadRequest("کد معتبر نمی باشد");
//            }


//            if (validationCode == info.ValadationCode)
//            {
//                var token = _jwtService.GenerateToken(user);
//                var refreshToken = _jwtService.GenerateRefreshToken(user);
//                _jwtService.SetTokenInCookie(token, HttpContext);
//                _jwtService.SetRefreshTokenInCookie(refreshToken, HttpContext);
//                user.RegisterCode = 0;
//                user.RegisterCodeTimer = null;
//                user.Status = UserStatus.Active;
//                _db.CyUser.Update(user);
//                await _db.SaveChangesAsync();
//                return Ok(new { token = token });
//            }
//            else
//            {
//                return BadRequest("کد تایید معتبر نمی باشد");
//            }
//        }

//        [HttpPost("sendRecoveryCode")]
//        public async Task<ActionResult> resendCode(string phoneNumber)
//        {
//            var validationCode = _random.Next(100000, 1000000);
//            var user = await _db.CyUser.Where(u=> u.IsVisible && ( u.CyUsNm == phoneNumber || u.CyUsNm == _smsService.ConvertPhoneNumber(phoneNumber))).FirstOrDefaultAsync();
//            if(user == null) return BadRequest("شماره همراه پیدا نشد");
//            user.RegisterCode = validationCode;
//            user.RegisterCodeTimer = DateTime.Now.AddMinutes(5);
//            await _db.SaveChangesAsync();
//            await _smsService.SendRegisterSmsAsync(phoneNumber, validationCode);
//            return Ok("کد بازیابی رمز عبور برای شما ارسال شد");
//        }

//        [HttpPut("recoverPassword")]
//        public async Task<ActionResult> recoverPassword(string phoneNumber, string password)
//        {
//            var user = await _db.CyUser.Where(u => u.IsVisible && (u.CyUsNm == phoneNumber || u.CyUsNm == _smsService.ConvertPhoneNumber(phoneNumber))).FirstOrDefaultAsync();
//            if (user == null) return BadRequest("شماره همراه پیدا نشد");

//            if (user.RegisterCodeTimer <= DateTime.Now) return BadRequest("کد بازیابی منقضی شده است");
            
//            user.CyHsPs = Crypto.EncryptStringAES(password);
//            user.RegisterCode = 0;
//            user.RegisterCodeTimer = null;
//            user.LastModified = DateTime.Now;
//            await _db.SaveChangesAsync();
//            return Ok("رمز عبور با موفقیت تغییر یافت");
//        }

//        //[Authorize]
//        [TypeAuthorize([UserType.Customer])]
//        [HttpPut("resetPassword")]
//        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDTO passwords)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null || !identity.IsAuthenticated)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var user = await _db.CyUser.Where(u=> u.IsVisible && u.ID == userid).FirstOrDefaultAsync();
//            if (user != null)
//            {
//                var isOldPasswordCorrect = _jwtService.isPasswordRight(passwords.OldPassword, user);

//                if (isOldPasswordCorrect)
//                {
//                    user.CyHsPs = Crypto.EncryptStringAES(passwords.NewPassword);
//                    user.LastModified = DateTime.UtcNow;
//                    _db.CyUser.Update(user);
//                    return Ok(await _db.SaveChangesAsync());
//                }
//                else
//                {
//                    return BadRequest("رمز عبور اشتباه می باشد");
//                }
//            }
//            return Unauthorized();
//        }

//        [TypeAuthorize([UserType.Customer, UserType.SysAdmin])]
//        [HttpGet("GetProfile")]
//        public async Task<ActionResult<ProfileDTO>> GetProfile()
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

//            var SelUsr = await _db.CyUser.Where(c => c.IsVisible && c.ID == userid).Include(c => c.CyProfile).FirstOrDefaultAsync();
//            if (SelUsr != null)
//            {
//                var basketItems = await _db.CyOrder.Where(o=> o.IsVisible && o.CyUserID == userid && o.Status == OrderStatus.InBasket).Select(o=>o.CyOrderItems.Where(i=>i.IsVisible)).FirstOrDefaultAsync();
//                if (SelUsr.CyProfile != null)
//                {
//                    var pfdto = _mapper.Map<ProfileDTO>(SelUsr.CyProfile);
//                    pfdto.Username = SelUsr.CyUsNm;
//                    pfdto.CyUserID = SelUsr.ID;
//                    pfdto.BasketCount = basketItems?.Count();
//                    return Ok(pfdto);
//                }
//                else
//                {
//                    return NotFound();
//                }
//            }
//            else
//            {
//                return NotFound(new { msg = "مشتری معتبر نمی باشد" });
//            }
//        }

//        //[Authorize]
//        [TypeAuthorize([UserType.Customer])]
//        [HttpPost("UpdateProfile")]
//        public async Task<IActionResult> UpdateProfile(ProfileDTO pfdt)
//        {
//            var identity = User.Identities.FirstOrDefault();
//            if (identity == null)
//                return Unauthorized("کاربر پیدا نشد");
//            var userid = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            
//            var SelProfile = await _db.CyUser.Where(c => c.IsVisible && c.ID == userid ).Select(c => c.CyProfile).FirstOrDefaultAsync();
//            if (SelProfile != null)
//            {
//                var pfdto = _mapper.Map<CyProfile>(pfdt);
//                pfdto.ID = SelProfile.ID;
//                pfdto.CreateDate = SelProfile.CreateDate;
//                pfdto.IsVisible = true;
//                pfdto.LastModified = DateTime.Now;
//                _db.Entry(SelProfile).CurrentValues.SetValues(pfdto);

//                return Ok(await _db.SaveChangesAsync());
//            }
//            else
//            {
//                return NotFound(new { msg = "پروفایل یافت نشد لطفا با پشتیبانی تماس بگیرید" });
//            }
//        }

//        [HttpGet("logout")]
//        public IActionResult Logout()
//        {
//            var options = new CookieOptions
//            {
//                HttpOnly = true,
//                Secure = true,
//                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
//                IsEssential = true,
//                SameSite = SameSiteMode.None,
//                Domain = ".chipyab.com"
//            };

//            Response.Cookies.Delete("accessToken", options);
//            Response.Cookies.Delete("refreshToken", options);

//            return Ok(new { msg = "Logged out successfully" });
//        }


//        [HttpGet("refreshToken")]
//        public IActionResult refreshToken()
//        {
//            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
//            {
//                return Unauthorized("Refresh token not found");
//            }

//            var principal = _jwtService.GetPrincipalFromExpiredToken(refreshToken);
//            if (principal == null)
//            {
//                return Unauthorized("Invalid refresh token");
//            }
//            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
//            if (userIdClaim == null)
//            {
//                return Unauthorized("User ID claim not found");
//            }
//            int userid = int.Parse(userIdClaim.Value);
//            var user = _db.CyUser.Where(u => u.IsVisible && u.ID == userid).FirstOrDefault();
//            if (user == null) { return BadRequest(); }

//            // Generate new tokens
//            var newAccessToken = _jwtService.GenerateToken(user);

//            //remove old token:
//            var options = new CookieOptions
//            {
//                HttpOnly = true,
//                Secure = true,
//                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
//                IsEssential = true,
//                SameSite = SameSiteMode.None,
//                Domain = ".chipyab.com"
//            };
//            Response.Cookies.Delete("accessToken", options);

//            // Set new token in cookies
//            _jwtService.SetTokenInCookie(newAccessToken, HttpContext);

//            return Ok(new { token = newAccessToken });
//        }
//    }
//}