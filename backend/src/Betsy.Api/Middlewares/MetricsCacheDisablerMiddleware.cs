using Microsoft.AspNetCore.OutputCaching;

namespace Betsy.Api.Middlewares;

public class MetricsCacheDisablerMiddleware
{
    private readonly RequestDelegate _next;

    public MetricsCacheDisablerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/metrics"))
        {
            var cachingFeature = context.Features.Get<IOutputCacheFeature>();
            if (cachingFeature != null)
            {
                cachingFeature.Context.AllowCacheLookup = false;
                cachingFeature.Context.AllowCacheStorage = false;
            }
        }

        await _next(context);
    }
}

public static class MetricsCacheDisablerMiddlewareExtensions
{
    public static IApplicationBuilder UseMetricsCacheDisabler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MetricsCacheDisablerMiddleware>();
    }
}
