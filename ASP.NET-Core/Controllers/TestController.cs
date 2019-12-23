using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace ASPNETCore.Controllers
{
    /// <summary>
    /// test controller, used to check JWT validity
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestController : ControllerBase
    {
        private IPrincipal _principal;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="principal"></param>
        public TestController(IPrincipal principal)
        {
            _principal = principal;
        }

        /// <summary>
        /// TestToken
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestToken")]
        public IActionResult TestToken()
        {
            string username = (_principal as ClaimsPrincipal)?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "unknown";

            return Ok($"Welcome: {username}");
        }
    }
}