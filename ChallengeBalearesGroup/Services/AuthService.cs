using ChallengeBalearesGroup.Models;
using ChallengeBalearesGroup.Repository;
using log4net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBalearesGroup.Services
{
    public interface IAuthService
    {
        Task<string> Login(User userLogin, List<string> errores);
        Task Logout(User userLogout, List<string> errores);
    }

    public class AuthService : IAuthService
    {
        //private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public AuthService(IPasswordHasher<User> passwordHasher, ITokenService tokenService, IUserRepository userRepository)
        {
            //_context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<string> Login(User userLogin, List<string> errores)
        {
            //var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            var user = await _userRepository.GetUser(userLogin.Username, userLogin.Correo);

            if (user == null)
            {
                errores.Add($"Usuario no encontrado.");
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, userLogin.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                errores.Add($"Contraseña incorrecta.");
                return null;
            }

            var token = _tokenService.GenerateToken(user);

            if(token == null)
            {
                errores.Add($"Error al generar el token.");
                return null;
            }

            return token;
        }

        public async Task Logout(User userLogout, List<string> errores)
        {
            //var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            var user = await _userRepository.GetUser(userLogout.Username, userLogout.Correo);
            if (user == null)
            {
                errores.Add($"Usuario no encontrado.");
                return;
            }

            return;
        }
    }
}
