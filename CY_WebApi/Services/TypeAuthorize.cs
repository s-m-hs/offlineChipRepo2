using CY_BM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CY_WebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CY_WebApi.Services
{
    public class TypeAuthorize : Attribute, IAsyncAuthorizationFilter
    {
        private readonly UserType[] _requiredUserTypes;

        public TypeAuthorize(UserType[] requiredUserType)
        {
            _requiredUserTypes = requiredUserType;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //Check if the user is authenticated
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {

                string subdomain = context.HttpContext.Request.Headers.Origin.ToString(); // Get the current subdomain

                string tokenName = subdomain.Contains("cyadmin") ? "AdminAccessTokenOffLine" : "accessTokenOffLine"; // Set the token name based on subdomain

                context.HttpContext.Request.Cookies.TryGetValue(tokenName, out var jwtToken); // Get the token by name


                if (string.IsNullOrEmpty(jwtToken))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                // Validate and decode the JWT token
                var handler = new JwtSecurityTokenHandler();
                ClaimsPrincipal principal;

                try
                {
                    // Validate the token (you should replace "your-secret-key" with your actual secret key)
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,

                        ValidateAudience = true,

                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3syx[o7E%8Ur#oW8sy<1UIDBU#yz}dOP(fGU#Uc:9q(R(:Us[fs")),

                        ValidateIssuerSigningKey = true,

                        ValidIssuer = "YourAPI",

                        ValidAudience = "YourAPIUsers",
                    };

                    principal = handler.ValidateToken(jwtToken, validationParameters, out _);
                    context.HttpContext.User = principal;
                }
                catch
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }


            var roltype = context.HttpContext.User.Identities.FirstOrDefault()?.RoleClaimType;
            // Retrieve the user type claim (assuming it is stored in claims)
            if (roltype != null)
            {
                var userTypeClaim = context.HttpContext.User.Claims
                                        .FirstOrDefault(c => c.Type == roltype)?.Value;

                /////var ii = context.HttpContext.User.Identities;///////

                if (Enum.TryParse<UserType>(userTypeClaim, out var userType))
                {
                    // Check if the user type is authenticated
                    if (!_requiredUserTypes.Contains(userType) && userType != UserType.SysAdmin)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                    // Continue with the request processing
                }
                else
                {
                    // If parsing fails, forbid access
                    context.Result = new ForbidResult();
                    return;
                }
            }
            await Task.CompletedTask;
        }
    }
}