using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using WebApp.Models;
using NLog.Web;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath);
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                   .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Reports/Index", "");
                
                options.Conventions.AuthorizeFolder("/Teams", "SuperAdminOnly");
                options.Conventions.AuthorizeFolder("/Reports");

                options.Conventions.AuthorizeFolder("/Assessors", "AdminOnly");
                options.Conventions.AuthorizeFolder("/AgedCareCenters", "AdminOnly");

                options.Conventions.AuthorizePage("/Questions/Create", "AdminOnly");
                options.Conventions.AuthorizePage("/Questions/Delete", "AdminOnly");
                options.Conventions.AuthorizePage("/Questions/Edit", "AdminOnly");

                options.Conventions.AuthorizePage("/AccreditationStandarts/Create", "AdminOnly");
                options.Conventions.AuthorizePage("/AccreditationStandarts/Delete", "AdminOnly");
                options.Conventions.AuthorizePage("/AccreditationStandarts/Edit", "AdminOnly");

                options.Conventions.AllowAnonymousToPage("/Reports/Index");

            });

            services.AddDbContext<AACCContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("SuperAdminOnly", policy => policy.RequireClaim("SuperAdmin"));
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            env.ConfigureNLog("nlog.config");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            loggerFactory.AddNLog();
        }
    }
}
