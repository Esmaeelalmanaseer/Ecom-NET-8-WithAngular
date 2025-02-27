using Ecom.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.API.Meddleware;

public class ExeptionsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _rateLimitWindow=TimeSpan.FromSeconds(30);
    public ExeptionsMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment, IMemoryCache memoryCache)
    {
        _next = next;
        _hostEnvironment = hostEnvironment;
        _memoryCache = memoryCache;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if(!IsRequestAllowed(context))
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";
                var Response = new ApiExeptions((int)HttpStatusCode.TooManyRequests, "To Many Requeset . please try again Later");
                await context.Response.WriteAsJsonAsync(Response);
            }
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var Response = _hostEnvironment.IsDevelopment() ?
                new ApiExeptions((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace!) :
                new ApiExeptions((int)HttpStatusCode.InternalServerError, ex.Message);
            var json = JsonSerializer.Serialize(Response);
            await context.Response.WriteAsJsonAsync(json);
        }
    }
    private bool IsRequestAllowed(HttpContext httpContext)
    {
        var IP = httpContext.Connection.RemoteIpAddress!.ToString();
        var cachKey = $"Rate{IP}";
        var DateNow=DateTime.Now;

        var (timesTamp, count) = _memoryCache.GetOrCreate(cachKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
            return (DateNow, 0);
        });
        if(timesTamp - DateNow <_rateLimitWindow)
        {
            if(count >=8)
            {
                return false;
            }
            _memoryCache.Set(cachKey, (timesTamp, count++),_rateLimitWindow);
        }
        else
        {
            _memoryCache.Set(cachKey, (timesTamp, count), _rateLimitWindow);
        }
        return true;
    }
}
