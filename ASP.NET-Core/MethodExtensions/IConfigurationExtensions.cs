using Microsoft.Extensions.Configuration;

namespace ASPNETCore.MethodExtensions
{
    using ASPNETCore.Configuration;

    /// <summary>
    /// IConfiguration method Extensions
    /// </summary>
    public static class IConfigurationExtensions
    {
        /// <summary>
        /// Get a configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static T Get<T>(this IConfiguration self, string section)
            where T : class, IAppSettingConfiguration
        {
            var s = self?
                 .GetSection(section);
            if(s != null)
                return s.Get<T>();
            return null;
        }
    }
}
