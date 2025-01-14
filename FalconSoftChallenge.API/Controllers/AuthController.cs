using FalconSoftChallenge.API.Security;
using Microsoft.AspNetCore.Mvc;

namespace FalconSoftChallenge.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userLoginModel)
        {
            if (userLoginModel == null) return BadRequest();

            try
            {
                var userToken = await _authService.LoginAndGenerateAccessToken(userLoginModel);

                return Ok(userToken);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
