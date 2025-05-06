using System.Net;

namespace FinAssist.Backend.Middlewares;

public class RealIpMiddleware
{
    private readonly RequestDelegate _next;

    public RealIpMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? ip = null;
        
        if (context.Request.Headers.TryGetValue("X-Real-IP", out var xRealIp))
        {
            ip = xRealIp.FirstOrDefault();
        }
        
        if (string.IsNullOrWhiteSpace(ip) && context.Request.Headers.TryGetValue("X-Forwarded-For", out var xff))
        {
            ip = xff.FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
        }
        
        if (string.IsNullOrWhiteSpace(ip))
        {
            ip = context.Connection.RemoteIpAddress?.ToString();
        }
        
        if (!IPAddress.TryParse(ip, out var parsed))
        {
            parsed = context.Connection.RemoteIpAddress;
        }

        context.Items["RealIp"] = parsed?.ToString();

        await _next(context);
    }
}