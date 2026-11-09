using Microsoft.AspNetCore.Mvc;

namespace CY_DM
{
    internal class UnauthorizedResult : IActionResult
    {
        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}