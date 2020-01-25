using ASPNETCore.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ASPNETCore
{
    using ASPNETCore.MethodExtensions;

    /// <summary>
    /// used to add JWT to project
    /// </summary>
    public static class RegisterJWT
    {
        private static JwtConfiguration original;
        /// <summary>
        /// Configure JWT. use in <seealso cref="Startup.ConfigureServices(IServiceCollection)"/>
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configuration"></param>
        public static void ConfigureJWTService(this IServiceCollection self, IConfiguration configuration)
        {
            self.Configure<JwtConfiguration>(configuration.GetSection(ConfigurationKeys.Jwt));
            original = configuration.Get<JwtConfiguration>(ConfigurationKeys.Jwt);

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
                        ValidIssuer = original.Issuer ?? string.Empty,
                        ValidAudience = original.Issuer ?? string.Empty,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(original.Key ?? string.Empty))
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

            using (var serviceScope = ServiceActivator.GetScope())
            {
                ILoggerFactory loggerFactory = serviceScope.ServiceProvider.GetService<ILoggerFactory>();
                IOptionsMonitor<JwtConfiguration> option = (IOptionsMonitor<JwtConfiguration>)serviceScope.ServiceProvider.GetService(typeof(IOptionsMonitor<JwtConfiguration>));
                

                ILogger logger = loggerFactory.CreateLogger(typeof(RegisterJWT));

                option.OnChange((o, s) =>
                {
                    if(original?.Issuer != option?.CurrentValue.Issuer || original?.Key != option?.CurrentValue.Key)
                        logger.LogWarning($"JwtConfiguration changed. Restart application to apply changes or discard them");
                    else
                        logger.LogInformation($"JwtConfiguration reset to original.");
                });
            }
        }
    }
}
