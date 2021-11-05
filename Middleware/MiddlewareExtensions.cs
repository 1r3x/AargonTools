using Microsoft.AspNetCore.Builder;

namespace AargonTools.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseIPFilter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IpFilterMiddleware>();
        }
    }
}
