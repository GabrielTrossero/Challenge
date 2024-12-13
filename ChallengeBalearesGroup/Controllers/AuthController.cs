﻿using ChallengeBalearesGroup.Models;
using ChallengeBalearesGroup.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core.Tokenizer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChallengeBalearesGroup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var errores = new List<string>();
            var result = await _userService.Register(user, errores);

            if (errores.Any())
            {
                return BadRequest(new { Errores = errores });
            }

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var errores = new List<string>();
            var token = await _authService.Login(user, errores);

            if (errores.Any())
            {
                return Unauthorized(new { Errores = errores });
            }

            return Ok(token);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] User user)
        {
            var errores = new List<string>();
            await _authService.Logout(user, errores);

            if (errores.Any())
            {
                return BadRequest(new { Errores = errores });
            }

            return Ok("Cierre de sesión exitoso.");
        }
    }
}
