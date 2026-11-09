using CY_BM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class AccessAuthorize : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _requiredPermissions;

    public AccessAuthorize(params string[] permissions)
    {
        _requiredPermissions = permissions;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // 1️⃣ اگر هنوز Authenticate نشده
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            string origin = context.HttpContext.Request.Headers.Origin.ToString();
            string tokenName = "AdminAccessTokenOffLine";
            //string tokenName = origin.Contains("cyadmin")
            //    ? "AdminAccessToken"
            //    : "accessToken";

            context.HttpContext.Request.Cookies.TryGetValue(tokenName, out var jwtToken);

            if (string.IsNullOrEmpty(jwtToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal;

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "YourAPI",
                    ValidAudience = "YourAPIUsers",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("3syx[o7E%8Ur#oW8sy<1UIDBU#yz}dOP(fGU#Uc:9q(R(:Us[fs"))
                    
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

        // 2️⃣ SysAdmin همیشه مجاز
        var roleClaimType = context.HttpContext.User.Identities.FirstOrDefault()?.RoleClaimType;
        var userTypeClaim = context.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == roleClaimType)?.Value;

        if (Enum.TryParse<UserType>(userTypeClaim, out var userType))
        {
            if (userType == UserType.SysAdmin)
                return;
        }

        // 3️⃣ Permissions از JWT
        var userPermissions = context.HttpContext.User.Claims
            .Where(c => c.Type == "Permission")
            .Select(c => c.Value)
            .ToList();

        // 4️⃣ OR logic
        bool hasAccess = _requiredPermissions.Any(p => userPermissions.Contains(p));

        if (!hasAccess)
        {
            context.Result = new ForbidResult();
            return;
        }

        await Task.CompletedTask;
    }
}
