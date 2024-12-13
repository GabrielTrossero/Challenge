using ChallengeBalearesGroup.Models;
using ChallengeBalearesGroup.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChallengeBalearesGroup.Services
{
    public interface IUserService
    {
        Task<User> Register(User user, List<string> errores);
    }

    public class UserService : IUserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;

        public UserService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<User> Register(User user, List<string> errores)
        {
            var existingUser = await _userRepository.GetUser(user.Username, user.Correo);
            if (existingUser != null)
            {
                errores.Add($"El usuario o el correo ya se encuentra registrado en el sistema.");
                return null;
            }

            var userRegister = new User
            {
                Username = user.Username,
                Correo = user.Correo,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Password = _passwordHasher.HashPassword(null, user.Password)
            };

            return await _userRepository.Create(userRegister);
        }
    }
}