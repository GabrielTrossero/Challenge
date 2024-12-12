using ChallengeBalearesGroup.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBalearesGroup.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly TokenService _tokenService;

        public AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, TokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _tokenService = tokenService;
        }

        public async Task<string> Login(string username, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return "Usuario no encontrado.";
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
            {
                return "Contraseña incorrecta.";
            }

            var token = _tokenService.GenerateToken(user);
            return token;
        }

        public async Task<string> Logout(string username)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return "Usuario no encontrado.";
            }

            // Lógica adicional: registrar cierre de sesión o invalidar token
            Console.WriteLine($"Usuario {username} cerró sesión.");
            return "Sesión cerrada exitosamente.";
        }
    }
}
