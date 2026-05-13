using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Autoparts.Api.Filters;

public class AuthorizedAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userId = context.HttpContext.Session.GetInt32("UserId");

        if (userId is null)
        {
            context.Result = new ObjectResult(new { message = "Необходима авторизация." })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }

        base.OnActionExecuting(context);
    }
}
