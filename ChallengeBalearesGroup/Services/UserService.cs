using ChallengeBalearesGroup.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBalearesGroup.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Register(User user)
        {
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Username == user.Username || u.Correo == user.Correo);
            if (existingUser != null)
            {
                return "Usuario o correo ya existe.";
            }

            var userRegister = new User
            {
                Username = user.Username,
                Correo = user.Correo,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Password = _passwordHasher.HashPassword(null, user.Password)
            };

            _context.User.Add(userRegister);
            await _context.SaveChangesAsync();

            return "Usuario registrado con éxito.";
        }
    }
}