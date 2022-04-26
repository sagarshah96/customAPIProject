using CustomAPIProject.ApplicationContext;
using CustomAPIProject.Extensions;
using CustomAPIProject.Filters;
using CustomAPIProject.Repository;
using CustomAPIProject.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public const string enUSCulture = "en-US";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("fr-FR")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: enUSCulture, uiCulture: enUSCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider { IndexOfCulture = 2, IndexofUICulture = 2 } };

            });

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("culture", typeof(LanguageRouteConstraint));
            });


            services.AddControllers();
            services.AddDbContext<DBContext>(o => o.UseSqlServer(Configuration.GetConnectionString("conectionStr")));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //services.AddTransient<CustomMiddleware>();
            //services.AddMemoryCache();

            services.AddScoped<_IRepository<Customer>, CustomerRepository<Customer>>();
            services.AddScoped<_IRepository<Login>, LoginRepository<Login>>();
            services.AddScoped<_ILoginService, LoginService>();

            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen();

            services.ConfigureOptions<ConfigureSwaggerOptions>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            var localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizeOptions.Value);

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<Middleware>();
            //app.UseMiddleware<CustomMiddleware>();

            // API Controller Route
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
               // endpoints.MapControllerRoute("default", "api/{culture:culture}/{controller=Language}/{action=Get}/{id?}");
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
