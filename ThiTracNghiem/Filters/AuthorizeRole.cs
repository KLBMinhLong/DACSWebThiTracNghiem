using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthorizeRoleAttribute : ActionFilterAttribute
{
    private readonly string _role;
    public AuthorizeRoleAttribute(string role) => _role = role;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userRole = context.HttpContext.Session.GetString("VaiTro");
        if (userRole != _role)
        {
            context.Result = new RedirectToActionResult("DangNhap", "TaiKhoan", null);
        }
    }
}
