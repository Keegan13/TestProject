using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.Data;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Host.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class DbSeedMiddleware
    {
        private readonly RequestDelegate _next;

        public DbSeedMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task InvokeAsync(HttpContext httpContext, ApplicationContext context)
        {
            if (
                await context.Projects.Skip(10).Take(1).CountAsync() == 0 ||
                await context.Developers.Skip(10).Take(1).CountAsync() == 0)
            {
                (new DataBaseSeed(context)).Initialize();
            }
            await _next(httpContext);
        }
    }
}
