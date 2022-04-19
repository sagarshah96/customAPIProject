using CustomAPIProject.ApplicationContext;
using CustomAPIProject.Filters;
using CustomAPIProject.Repository;
using CustomAPIProject.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject
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

            services.AddControllers();
            services.AddDbContext<DBContext>(o => o.UseSqlServer(Configuration.GetConnectionString("conectionStr")));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //services.AddTransient<CustomMiddleware>();
            //services.AddMemoryCache();

            services.AddScoped<_IRepository<Customer>, CustomerRepository<Customer>>();
            services.AddScoped<_IRepository<Login>, LoginRepository<Login>>();
            services.AddScoped<_ILoginService, LoginService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomAPIProject", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomAPIProject v1"));
            }

            #region Default Exception
            // Default Exception

            //app.UseExceptionHandler(a => a.Run(async context =>
            //{
            //    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            //    var exception = exceptionHandlerPathFeature.Error;
            //    await context.Response.WriteAsJsonAsync(new { error = exception.Message, path = exceptionHandlerPathFeature.Path });
            //}));
            #endregion

            #region Rewrite URL
            //var rewrite = new RewriteOptions().AddRewrite("api/Customer/GetAllCustomers", "api/Customer/GetAllCustomersv2", true);
            //app.UseRewriter(rewrite);
            #endregion

            #region Global Caching
            //app.Use(async (context, next) =>
            //{
            //    context.Response.GetTypedHeaders().CacheControl =
            //     new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //     {
            //         Public = true,
            //         MaxAge = TimeSpan.FromSeconds(60),
            //     };
            //    await next();
            //});
            #endregion

            // Custom Exception Middleware
            app.UseMiddleware<HandledExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<Middleware>();
            //app.UseMiddleware<CustomMiddleware>();

            // API Controller Route
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Custom Controller Route
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Customer}/{action=GetAllCustomers}/{id?}");
            //});
            #endregion
        }

    }
}
