using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Services;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomDbContext(this IServiceCollection services, bool includeSeed = false)
        {
            var provider = services.BuildServiceProvider();
            var config = provider.GetRequiredService<IConfiguration>();
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        }
        public static void AddProjectManager(this IServiceCollection services)
        {
            services.AddScoped<ProjectManagerService>();
        }
    }
}
