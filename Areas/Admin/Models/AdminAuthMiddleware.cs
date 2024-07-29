using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class AdminAuthMiddleware
{
    private readonly RequestDelegate _next;

    public AdminAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/admin") &&
            !context.Request.Path.Equals("/admin/staff/login", StringComparison.OrdinalIgnoreCase))
        {
            if (context.Session.GetString("AdminEmail") == null)
            {
                context.Session.SetString("ReturnUrl", context.Request.Path + context.Request.QueryString);
                context.Response.Redirect("/admin/staff/login");
                return;
            }
        }

        await _next(context);
    }
}
