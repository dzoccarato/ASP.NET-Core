using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ASPNETCore.Services
{
    /// <summary>
    /// Used to register to startup all services
    /// </summary>
    public static class Register
    {
        /// <summary>
        /// used in <seealso cref="Startup.ConfigureServices(IServiceCollection)"/>
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configuration"></param>
        public static void ConfigureAppServices(this IServiceCollection self, IConfiguration configuration)
        {
            self.AddSingleton(typeof(ILoginService), typeof(LoginService));
        }
    }
}
