using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace ASPNETCore
{
    /// <summary>
    /// used to add Swagger to project
    /// </summary>
    public static class RegisterSwagger
    {
        /// <summary>
        /// Configure JWT. use in <seealso cref="Startup.ConfigureServices(IServiceCollection)"/>
        /// </summary>
        /// <param name="self"></param>
        public static void ConfigureSwaggerService(this IServiceCollection self)
        {
#if DEBUG
            // Register the Swagger generator, defining 1 or more Swagger documents
            self.AddSwaggerGen(c =>
           {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ASPNETCore API",
                    Version = "v1",
                    Description = "Full documentation to ASPNETCore public API",
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
#endif
        }

        /// <summary>
        /// Configure Swagger. use in <seealso cref="Startup.Configure(IApplicationBuilder, Microsoft.AspNetCore.Hosting.IWebHostEnvironment)"/>
        /// </summary>
        /// <param name="self"></param>
        public static void ConfigureSwagger(this IApplicationBuilder self)
        {
#if DEBUG
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            self.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            self.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASPNETCore API V1");
                c.RoutePrefix = "swagger/ui";
            });
#endif
        }
    }
}
