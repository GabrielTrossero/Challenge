using ChallengeBalearesGroup.Models;
using ChallengeBalearesGroup.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeBalearesGroup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService userService;
        private readonly AuthService authService;

        public AuthController(UserService userService, AuthService authService)
        {
            this.userService = userService;
            this.authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var result = await this.userService.Register(user);
            if (result.Contains("éxito"))
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var token = await this.authService.Login(user.Username, user.Password);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Error al generar el token.");
            }

            return Ok(token);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] User user)
        {
            await this.authService.Logout(user.Username);
            return Ok("Cierre de sesión exitoso.");
        }
    }
}
