using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace AspNetCoreJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestController : ControllerBase
    {
        private IPrincipal _principal;
        public TestController(IPrincipal principal)
        {
            _principal = principal;
        }

        [HttpGet("TestToken")]
        public IActionResult TestToken()
        {
            string username = (_principal as ClaimsPrincipal)?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? "unknown";

            return Ok($"Welcome: {username}");
        }
    }
}