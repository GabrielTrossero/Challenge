using ChallengeBalearesGroup.Models;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace ChallengeBalearesGroup.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> Filter(int? id, string? username, string? correo, string? nombre, string? apellido);
        Task<User> GetUser(string? username, string? correo);
        Task<User> Create(User user);
    }

    public class UserRepository : IUserRepository
    {
        private static readonly ILog log4net = LogManager.GetLogger(typeof(ContactRepository));
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<User> Create(User user)
        {
            try
            {
                _context.User.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                log4net.Error("Error al crear el Usuario: ", ex);
                throw;
            }
        }


        public async Task<IEnumerable<User>> Filter(int? id, string? username, string? correo, string? nombre, string? apellido)
        {
            IQueryable<User> query = _context.User;

            if (id > 0)
            {
                query = query.Where(c => c.Id == id);
            }

            if (!string.IsNullOrWhiteSpace(username))
            {
                query = query.Where(c => EF.Functions.Like(c.Username, $"%{username}%"));
            }

            if (!string.IsNullOrWhiteSpace(correo))
            {
                query = query.Where(c => EF.Functions.Like(c.Correo, $"%{correo}%"));
            }

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(c => EF.Functions.Like(c.Nombre, $"%{nombre}%"));
            }

            if (!string.IsNullOrWhiteSpace(apellido))
            {
                query = query.Where(c => EF.Functions.Like(c.Apellido, $"%{apellido}%"));
            }


            var users = await query.ToListAsync();

            return users;
        }


        public async Task<User> GetUser(string? username, string? correo)
        {
            IQueryable<User> query = _context.User;

            query = query.Where(u => (username != null && u.Username == username)
                                  || (correo != null && u.Correo == correo));

            var user = await query.FirstOrDefaultAsync();

            return user;
        }
    }
}
