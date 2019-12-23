using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ASPNETCore
{
    /// <summary>
    /// used to add JWT to project
    /// </summary>
    public static class RegisterJWT
    {
        /// <summary>
        /// Configure JWT. use in <seealso cref="Startup.ConfigureServices(IServiceCollection)"/>
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configuration"></param>
        public static void ConfigureJWTService(this IServiceCollection self, IConfiguration configuration)
        {
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            self.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });
        }

        /// <summary>
        /// Configure JWT. use in <seealso cref="Startup.Configure(IApplicationBuilder, Microsoft.AspNetCore.Hosting.IWebHostEnvironment)"/>
        /// </summary>
        /// <param name="self"></param>
        public static void ConfigureJWT(this IApplicationBuilder self)
        {
            self.UseAuthentication();
        }
    }
}
