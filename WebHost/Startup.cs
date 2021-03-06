using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Extensions;
using AutoMapper;
using Host.Extensions;
using Microsoft.AspNetCore.Http;
using System.IO;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Host API", Version = "v1" });
            });

            services.AddCustomDbContext(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging());

            services.AddEFCoreRepositories();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(_next =>
            {
                return async (context) =>
                {
                    var fact= context.RequestServices.GetService<ILoggerFactory>();
                    
                    await _next.Invoke(context);
                };

            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //Uncomment this to turn on seed (Note! works only on emtpy db)
                //app.UseDBSeed();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseHttpsRedirection();

            app.UseStaticFiles();

            
            app.UseSpaStaticFiles();

           
            //app.UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>();
            //app.UseMvc(routes =>
            //{
            //    //routes.MapRoute(
            //    //    name: "default",
            //    //    template: "{Controller?}/{Action?}",
            //    //    defaults: new { controller = "Home", Action = "Index" });
            //    routes.MapRoute(
            //        name: "main",
            //        template: "api/{Controller}/{name}",
            //        defaults: new { action = "Single" });
            //    routes.MapRoute(
            //        name: "manage",
            //        template: "api/{controller}s/{action}/{name?}"
            //       );
            //});


            app.UseMvcWithDefaultRoute();


            // fork pipeline with swagger middleware if path startd with swagger
            app.Map("/swagger", (x) =>
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });

            });


            // ng serve
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            // return compiled SPA  index page

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });
        }
    }
}
