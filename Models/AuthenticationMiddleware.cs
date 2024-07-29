using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.ToString().ToLower();

        // Nếu người dùng chưa đăng nhập và không yêu cầu truy cập trang Login hoặc Register
        if (!context.Session.GetInt32("UserId").HasValue && !path.Contains("/account/login") && !path.Contains("/account/register"))
        {
            context.Response.Redirect("/Account/Login");
            return;
        }

        await _next(context);
    }
}
