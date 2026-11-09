using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CY_WebApi.Services
{
    public class TypeAuthorizeB : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _permissions;

        public TypeAuthorizeB(params string[] permissions)
        {
            _permissions = permissions;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // بدست آوردن Permissionهای کاربر از دیتابیس یا Claims
            var userPermissions = GetUserPermissions(context.HttpContext.User);

            bool hasPermission = _permissions.Any(p => userPermissions.Contains(p));

            if (!hasPermission)
            {
                context.Result = new UnauthorizedResult();
            }

            await Task.CompletedTask;
        }

        private List<string> GetUserPermissions(ClaimsPrincipal user)
        {
            return user.Claims
                .Where(x => x.Type == "Permission")
                .Select(x => x.Value)
                .ToList();
        }
    }
}
