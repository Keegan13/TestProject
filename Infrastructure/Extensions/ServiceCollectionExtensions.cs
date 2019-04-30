using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Abstractions;
using System;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomDbContext(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<ApplicationContext>(options);
        }

        public static void AddEFCoreRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDeveloperRepository, DeveloperRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectAssignments, ProjectAssignments>();
            services.AddScoped<ITagRepository, TagRepository>();
        }
    }
}
