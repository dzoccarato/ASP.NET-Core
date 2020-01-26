namespace ASPNETCore.Configuration
{
    /// <summary>
    /// Wrapper for ServiceSettings section from configuration file.
    /// </summary>
    public class ServiceSettings : IAppSettingConfiguration
    {
        /// <summary>
        /// Available urls
        /// </summary>
        public string[] Urls { get; set; }
    }
}
