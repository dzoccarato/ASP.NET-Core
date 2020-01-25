using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASPNETCore.Services
{
    using ASPNETCore.Configuration;
    using ASPNETCore.Dto;

    /// <summary>
    /// Implements ILoginService
    /// </summary>
    public class LoginService: ILoginService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<LoginService> _logger;
        private readonly IOptionsMonitor<JwtConfiguration> _options;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public LoginService(IConfiguration config, ILogger<LoginService> logger, IOptionsMonitor<JwtConfiguration> options)
        {
            _config = config;
            _logger = logger;
            _options = options;
        }

        /// <summary>
        /// Mocked autentication service
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool Authenticate(UserLogin user)
        {
            return string.Equals(user.UserName, "John Doe", StringComparison.InvariantCultureIgnoreCase)
                    && string.Equals(user.Password, "123", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// generate a basic JWT token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GenerateJWT(UserLogin user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.CurrentValue.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            };

            var token = new JwtSecurityToken(_options.CurrentValue.Issuer,
              _options.CurrentValue.Issuer,
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
