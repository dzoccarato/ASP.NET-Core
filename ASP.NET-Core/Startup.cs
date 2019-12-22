using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCoreJWT
{
    using AspNetCoreJWT.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.OpenApi.Models;
    using System.Security.Principal;

    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// IConfiguration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AspNetCoreJWT API",
                    Version = "v1",
                    Description = "Full documentation to AspNetCoreJWT public API",
                    Contact = new OpenApiContact
                    {
                        Name = "Zoccarato Davide",
                        Email = "davide@davidezoccarato.cloud",
                        Url = new Uri("https://www.davidezoccarato.cloud/")
                    },
                });
#pragma warning disable CS0618 // Type or member is obsolete
                c.DescribeAllEnumsAsStrings();
#pragma warning restore CS0618 // Type or member is obsolete
                c.IncludeXmlComments(string.Format(@"{0}\ASP.NET-Core.xml", System.AppDomain.CurrentDomain.BaseDirectory));                
                c.IgnoreObsoleteProperties();
            });

            //
            services.ConfigureJWT(Configuration);
            services.ConfigureAppServices(Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseJWT();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region SWAGGER
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetCoreJWT API V1");
                c.RoutePrefix = "swagger/ui";
            });
            #endregion
        }
    }
}
