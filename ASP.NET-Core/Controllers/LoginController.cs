using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreJWT.Controllers
{
    using AspNetCoreJWT.Dto;
    using AspNetCoreJWT.Services;

    /// <summary>
    /// login controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _service;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="service"></param>
        public LoginController(ILoginService service)
        {
            _service = service;
        }

        /// <summary>
        /// login: validate user credentials and generate JWT auth token.
        /// AllowAnonymous
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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