using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ASPNETCore.Services
{
    using ASPNETCore.Dto;

    /// <summary>
    /// Implements ILoginService
    /// </summary>
    public class LoginService: ILoginService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<LoginService> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        public LoginService(IConfiguration config, ILogger<LoginService> logger)
        {
            _config = config;
            _logger = logger;
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
