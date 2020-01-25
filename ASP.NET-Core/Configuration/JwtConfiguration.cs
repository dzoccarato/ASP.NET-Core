using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ASPNETCore.Configuration
{
    /// <summary>
    /// Wrapper for Jwt section from configuration file.
    /// Used as <seealso cref="IOptionsMonitor&lt;JwtConfiguration&gt;"/>
    /// </summary>
    public class JwtConfiguration : IAppSettingConfiguration
    {
        /// <summary>
        /// Jwt Key. Used to create <seealso cref="SymmetricSecurityKey"/> 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Jwt Issuer. Used to create <seealso cref="JwtSecurityToken"/> 
        /// </summary>
        public string Issuer { get; set; }
    }
}
