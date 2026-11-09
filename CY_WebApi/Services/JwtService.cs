using CY_BM;
using CY_DM;
using CY_WebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CY_WebApi.Models;
using Microsoft.EntityFrameworkCore;
using static Google.Cloud.RecaptchaEnterprise.V1.TransactionData.Types;

namespace CY_WebApi.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IConfiguration configuration)

        {

            _jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

        }

        public string GenerateToken(CyUser user)

        {

            var claims = new List<Claim>

        {


            new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),

            new Claim(ClaimTypes.Name, user.CyUsNm),

            new Claim(ClaimTypes.Role, user.userType.ToString())

            // Add additional claims as needed (e.g., roles)

        };
            if (user.AccessTable != null)
            {
                var accessProps = typeof(AccessTable).GetProperties()
                    .Where(p => p.PropertyType == typeof(bool));

                foreach (var prop in accessProps)
                {
                    var value = (bool)prop.GetValue(user.AccessTable)!;
                    if (value)
                    {
                        claims.Add(new Claim("Permission", prop.Name));
                    }
                }
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor

            {

                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryInMinutes),

                SigningCredentials = credentials,

                Issuer = _jwtSettings.Issuer,

                Audience = _jwtSettings.Audience

            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);

        }

        public string GenerateRefreshToken(CyUser user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.CyUsNm),
                new Claim(ClaimTypes.Role, user.userType.ToString())

                // Add additional claims as needed (e.g., roles)
            };
            if (user.AccessTable != null)
            {
                var accessProps = typeof(AccessTable).GetProperties()
                    .Where(p => p.PropertyType == typeof(bool));

                foreach (var prop in accessProps)
                {
                    var value = (bool)prop.GetValue(user.AccessTable)!;
                    if (value)
                    {
                        claims.Add(new Claim("Permission", prop.Name));
                    }
                }
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(double.Parse(_jwtSettings.RefreshTokenExpirationDays)),
                SigningCredentials = credentials,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }



        public async Task<CyUser?> IsValidUser(LoginModel login, CyContext _db)
        {
            // Replace this with your actual user validation logic (e.g., compare with a database)
            //var SelU = _db.CyUser.Where(c => c.CyUsNm == login.Un && c.Status == UserStatus.Active).FirstOrDefault();

            //return SelU != null && login.Pw == SelU.CyHsPs ? SelU : null; // Example (replace with real logic)

            var somResu = await CreateAdminMaster(_db);
            // Replace this with your actual user validation logic (e.g., compare with a database)
            var SelU = await _db.CyUser.Where(c => c.CyUsNm == login.Un && c.IsVisible &&
                                                  (c.Status == UserStatus.Active || c.Status == UserStatus.UnConfirmed) 
                                                   ).FirstOrDefaultAsync();

            if (SelU != null)
            {
                var DbPass = Crypto.DecryptStringAES(SelU.CyHsPs);
                var HashPass = Crypto.GetStringSha512Hash(DbPass);

                if (HashPass == login.Pw)
                {
                    return SelU;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<CyUser?> IsValidUserForCustomer(LoginModel login, CyContext _db)
        {
            // Replace this with your actual user validation logic (e.g., compare with a database)
            //var SelU = _db.CyUser.Where(c => c.CyUsNm == login.Un && c.Status == UserStatus.Active).FirstOrDefault();

            //return SelU != null && login.Pw == SelU.CyHsPs ? SelU : null; // Example (replace with real logic)

            var somResu = await CreateAdminMaster(_db);
            // Replace this with your actual user validation logic (e.g., compare with a database)
            var SelU = await _db.CyUser.Where(c => c.CyUsNm == login.Un && c.IsVisible &&
                                                  (c.Status == UserStatus.Active || c.Status == UserStatus.UnConfirmed)
                                                   ).FirstOrDefaultAsync();

            if (SelU != null)
            {
                var DbPass = Crypto.DecryptStringAES(SelU.CyHsPs);
                var HashPass = Crypto.GetStringSha512Hash(DbPass);

                if (HashPass == login.Pw)
                {
                    return SelU;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public bool isPasswordRight(string password, CyUser user)
        {
            var DbPass = Crypto.DecryptStringAES(user.CyHsPs);
            var HashPass = Crypto.GetStringSha512Hash(DbPass);

            if (HashPass == password)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CreateAdminMaster(CyContext _db)
        {
            var adminU = await _db.CyUser.Where(c => c.CyUsNm == "AdminSys").FirstOrDefaultAsync();
            if (adminU == null)
            {

                adminU = new CyUser
                {
                    CyHsPs = Crypto.EncryptStringAES("786Adm!n313"),
                    CyUsNm = "AdminSys",
                    CreateDate = DateTime.Now,
                    IsVisible = true,
                    MaxWrongPass = 8,
                    Status = UserStatus.Active,
                    WrongPass = 0,
                    userType = UserType.SysAdmin,
                    
                };
                var _out2 = _db.CyUser.Add(adminU);

                int j = await _db.SaveChangesAsync();
                return j > 0;
            }
            else
            {
                return false;
            }

        }


        public void SetTokenInCookie(string token, HttpContext context, string domain = "client")
        {
            var name = domain == "client" ? "accessTokenOffLine" : "AdminAccessTokenOffLine";
            var host = context.Request.Host.Host;
            var isLocalhost = host.Contains("localhost") || host.Contains("127.0.0.1");

            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(70),
                HttpOnly = true,
                IsEssential = true,
                Secure = true, // در حالت توسعه اگه HTTPS نداری باید false بشه، ولی ترجیحاً با HTTPS تست بگیر
                SameSite = SameSiteMode.None,
            
            };
            if (!isLocalhost)
            {
                cookieOptions.Domain = DomainConfig.Domain; // توجه کن دامنه باید با . شروع بشه برای زیر دامنه‌ها
            }

            context.Response.Cookies.Append(name, token, cookieOptions);

        }

        public void SetRefreshTokenInCookie(string token, HttpContext context, string domain = "client")
        {
            var name = domain == "client" ? "refreshTokenOffLine" : "AdminRefreshTokenOffLine";
            var host = context.Request.Host.Host;
            var isLocalhost = host.Contains("localhost") || host.Contains("127.0.0.1");

            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(14),
                HttpOnly = true,
                IsEssential = true,
                Secure = true, // در حالت توسعه اگه HTTPS نداری باید false بشه، ولی ترجیحاً با HTTPS تست بگیر
                SameSite = SameSiteMode.None,
            };
            if (!isLocalhost)
            {
                cookieOptions.Domain = DomainConfig.Domain; // توجه کن دامنه باید با . شروع بشه برای زیر دامنه‌ها
            }

            context.Response.Cookies.Append(name, token, cookieOptions);

        }


        //public void SetTokenInCookie(string token, HttpContext context, string domain = "client")
        //{
        //    var name = domain == "client" ? "accessTokenOffLine" : "AdminAccessTokenOffLine";

        //    context.Response.Cookies.Append(name, token, new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        Expires = DateTimeOffset.UtcNow.AddMinutes(30),
        //        IsEssential = true,
        //        SameSite = SameSiteMode.None,
        //        Domain = ".chipyab.com"
        //    });
        //}

        //public void SetRefreshTokenInCookie(string token, HttpContext context, string domain = "client")
        //{
        //    var name = domain == "client" ? "refreshTokenOffLine" : "AdminRefreshTokenOffLine";

        //    context.Response.Cookies.Append(name, token, new CookieOptions
        //    {
        //        Expires = DateTimeOffset.UtcNow.AddDays(7),
        //        HttpOnly = true,
        //        IsEssential = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.None,
        //        Domain = ".chipyab.com"
        //    });
        //}

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false // Ignore expiration during validation
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
                if (securityToken is not JwtSecurityToken jwt || !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                {
                    return null;
                }
                return principal;
            }
            catch
            {
                return null;
            }
        }

    }
}
