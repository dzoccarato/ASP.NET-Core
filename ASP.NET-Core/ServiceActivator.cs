using Microsoft.Extensions.DependencyInjection;
using System;

namespace ASPNETCore
{
    /// <summary>
    /// Add static service resolver to use when dependencies injection is not available
    /// </summary>
    public class ServiceActivator
    {
        internal static IServiceProvider _serviceProvider = null;

        /// <summary>
        /// Configure ServiceActivator with full serviceProvider
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Create a scope where use this ServiceActivator
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceScope GetScope(IServiceProvider serviceProvider = null)
        {
            var provider = serviceProvider ?? _serviceProvider;
            return provider?
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
        }

        /// <summary>
        /// retrive a service of type T from injected services
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static T GetService<T>(IServiceProvider serviceProvider = null)
        {
            var provider = serviceProvider ?? _serviceProvider;
            return provider == null ? default : provider.GetService<T>();
        }
    }
}
