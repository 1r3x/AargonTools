using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text.Json;
using System.Threading.Tasks;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;

    public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpointsToRateLimit = new[] { "/api/CreditCards/TCRProcessCC", "/api/CreditCards/ProcessCcV2" };

        if (Array.Exists(endpointsToRateLimit, endpoint => context.Request.Path.StartsWithSegments(endpoint)))
        {
            var request = context.Request;
            request.EnableBuffering();

            // Read the request body
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = System.Text.Encoding.UTF8.GetString(buffer);
            request.Body.Position = 0;

            // Generate a unique key for the request
            var cacheKey = $"RateLimit-{requestBody.GetHashCode()}";

            // Check if the same request has been processed within the last 5 minutes
            if (_cache.TryGetValue(cacheKey, out _))
            {
                string debtorAcct = "";
                using (JsonDocument doc = JsonDocument.Parse(requestBody))
                {
                    JsonElement root = doc.RootElement;
                    debtorAcct = root.GetProperty("debtorAcct").GetString();
                }
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                Serilog.Log.Warning("Duplicate request detected: {debtorAcct}", debtorAcct);
                await context.Response.WriteAsync("Duplicate request detected. Please wait before retrying.");
                return;
            }

            // Store the request in the cache with a 100 sec expiration
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(101)
            };
            _cache.Set(cacheKey, true, cacheEntryOptions);
        }

        await _next(context);
    }
}