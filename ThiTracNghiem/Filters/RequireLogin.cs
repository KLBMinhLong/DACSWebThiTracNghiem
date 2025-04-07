using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class RequireLoginAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var isLoggedIn = context.HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(isLoggedIn))
        {
            context.Result = new RedirectToActionResult("DangNhap", "TaiKhoan", null);
        }
    }
}
