using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Autoparts.Api.Filters;

public class AdminModAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var role = context.HttpContext.Session.GetString("UserRole");

        if (role != "Admin")
        {
            context.Result = new ObjectResult(new { message = "Доступ запрещён. Требуется роль Admin." })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        base.OnActionExecuting(context);
    }
}
