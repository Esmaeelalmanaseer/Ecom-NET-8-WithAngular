using Ecom.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace Ecom.API.Meddleware;

public class ExeptionsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

    public ExeptionsMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment, IMemoryCache memoryCache)
    {
        _next = next;
        _hostEnvironment = hostEnvironment;
        _memoryCache = memoryCache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ApplySecurity(context);

        if (!IsRequestAllowed(context))
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";
                var response = new ApiExeptions((int)HttpStatusCode.TooManyRequests, "To many requests. Please try again later.");
                await context.Response.WriteAsJsonAsync(response);
            }
            return; 
        }

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _hostEnvironment.IsDevelopment()
                    ? new ApiExeptions((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace!)
                    : new ApiExeptions((int)HttpStatusCode.InternalServerError, "An internal server error occurred.");
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

    private bool IsRequestAllowed(HttpContext httpContext)
    {
        var ip = httpContext.Connection.RemoteIpAddress!.ToString();
        var cacheKey = $"RateLimit_{ip}";
        var now = DateTime.Now;

        if (!_memoryCache.TryGetValue(cacheKey, out (DateTime timestamp, int count) cacheEntry))
        {
            cacheEntry = (now, 1);
            _memoryCache.Set(cacheKey, cacheEntry, _rateLimitWindow);
            return true;
        }

        if (now - cacheEntry.timestamp < _rateLimitWindow)
        {
            if (cacheEntry.count >= 8)
            {
                return false;
            }

            cacheEntry.count++;
            _memoryCache.Set(cacheKey, cacheEntry, _rateLimitWindow);
            return true;
        }
        else
        {
            cacheEntry = (now, 1);
            _memoryCache.Set(cacheKey, cacheEntry, _rateLimitWindow);
            return true;
        }
    }

    private void ApplySecurity(HttpContext httpContext)
    {
        httpContext.Response.Headers["X-Content-Type-Options"] = "nosniff";
        httpContext.Response.Headers["X-XSS-Protection"] = "1; mode=block";
        httpContext.Response.Headers["X-Frame-Options"] = "DENY";
    }
}