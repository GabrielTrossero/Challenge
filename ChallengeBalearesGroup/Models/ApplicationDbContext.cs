using ChallengeBalearesGroup.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    // Constructor para recibir la configuración del DbContext
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Definir DbSet para cada entidad que represente una tabla en la base de datos
    public DbSet<Contact> Contact { get; set; }
    public DbSet<User> User { get; set; }
}
