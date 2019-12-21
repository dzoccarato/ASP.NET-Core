using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreJWT.Controllers
{
    using AspNetCoreJWT.Dto;
    using AspNetCoreJWT.Services;

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody]UserLogin user)
        {
            IActionResult response = Unauthorized();

            if (_service.Authenticate(user))
            {
                var tokenString = _service.GenerateJWT(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

    }
}