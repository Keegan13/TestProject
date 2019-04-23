using Host.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Host.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDBSeed(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbSeedMiddleware>();
        }
    }
}
