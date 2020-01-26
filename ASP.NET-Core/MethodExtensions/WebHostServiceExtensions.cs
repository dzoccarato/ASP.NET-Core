using Microsoft.AspNetCore.Hosting;
using System.ServiceProcess;

namespace ASPNETCore.MethodExtensions
{
    /// <summary>
    /// WebHostServiceExtensions
    /// </summary>
    public static class WebHostServiceExtensions
    {
        /// <summary>
        /// RunAsCustomService
        /// </summary>
        /// <param name="host"></param>
        public static void RunAsCustomService(this IWebHost host)
        {
            var webHostService = new CustomWebHostService(host);
            ServiceBase.Run(webHostService);
        }
    }
}
