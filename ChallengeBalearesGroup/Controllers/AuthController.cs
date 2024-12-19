using ChallengeBalearesGroup.Models;
using ChallengeBalearesGroup.Models.DTO;
using ChallengeBalearesGroup.Models.Mappers;
using ChallengeBalearesGroup.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core.Tokenizer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChallengeBalearesGroup.Controllers
{
    /// <summary>
    /// Controlador para la autenticación y gestión de usuarios.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor de AuthController.
        /// </summary>
        /// <param name="userService">Servicio de gestión de usuarios.</param>
        /// <param name="authService">Servicio de autenticación.</param>
        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="userRegisterDTO">Datos del usuario a registrar.</param>
        /// <returns>Resultado de la operación de registro.</returns>
        /// <response code="200">Registro exitoso.</response>
        /// <response code="400">Errores encontrados en la operación de registro.</response>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            var errores = new List<string>();
            var user = UserMapper.MapUserRegisterDTOToUser(userRegisterDTO);
            var result = await _userService.Register(user, errores);

            if (errores.Any())
            {
                return BadRequest(new { Errores = errores });
            }

            return Ok(result);
        }

        /// <summary>
        /// Inicia sesión para un usuario existente.
        /// </summary>
        /// <param name="userLoginDTO">Credenciales de inicio de sesión.</param>
        /// <returns>Token de acceso si el inicio de sesión es exitoso.</returns>
        /// <response code="200">Inicio de sesión exitoso.</response>
        /// <response code="401">Credenciales incorrectas.</response>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(object), 401)]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var errores = new List<string>();
            var user = UserMapper.MapUserLoginDTOToUser(userLoginDTO);
            var token = await _authService.Login(user, errores);

            if (errores.Any())
            {
                return Unauthorized(new { Errores = errores });
            }

            return Ok(token);
        }

        /// <summary>
        /// Cierra la sesión del usuario actual.
        /// </summary>
        /// <param name="userLogoutDTO">Información de cierre de sesión.</param>
        /// <returns>Mensaje de éxito si el cierre de sesión es correcto.</returns>
        /// <response code="200">Cierre de sesión exitoso.</response>
        /// <response code="400">Errores encontrados durante el cierre de sesión.</response>
        [HttpPost("Logout")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(object), 400)]
        public async Task<IActionResult> Logout([FromBody] UserLogoutDTO userLogoutDTO)
        {
            var errores = new List<string>();
            var user = UserMapper.MapUserLogoutDTOToUser(userLogoutDTO);
            await _authService.Logout(user, errores);

            if (errores.Any())
            {
                return BadRequest(new { Errores = errores });
            }

            return Ok("Cierre de sesión exitoso.");
        }
    }
}
