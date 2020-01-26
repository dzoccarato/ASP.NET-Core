using Microsoft.Extensions.Configuration;

namespace ASPNETCore.Configuration
{
    /// <summary>
    /// ConfigurationUtils
    /// </summary>
    public static class ConfigurationUtils
    {
        /// <summary>
        /// BuildConfigurations
        /// </summary>
        /// <returns></returns>
        public static IConfigurationRoot BuildConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.production.json", optional: true, reloadOnChange: true)
                .Build();

    }
}
