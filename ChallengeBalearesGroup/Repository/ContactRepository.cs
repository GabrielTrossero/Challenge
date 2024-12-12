using ChallengeBalearesGroup.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using log4net;

namespace ChallengeBalearesGroup.Repository
{
    public interface IContactRepository
    {
        Task<Contact> Create(Contact contact, IFormFile? imagen);
        Task<IEnumerable<Contact>> Filter(int id, string email, string telefono, string direccion);
        (Stream Imagen, string TipoContenido)? GetImage(string nombreArchivo);
    }

    public class ContactRepository : IContactRepository
    {
        private static readonly ILog log4net = LogManager.GetLogger(typeof(ContactRepository));
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;


        public ContactRepository(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<Contact> Create(Contact contact, IFormFile? imagen)
        {
            try
            {
                if (imagen != null)
                {
                    // Definir la carpeta de almacenamiento
                    string carpetaImagenes = Path.Combine(_env.WebRootPath, "contact");

                    // Crear la carpeta si no existe
                    if (!Directory.Exists(carpetaImagenes))
                        Directory.CreateDirectory(carpetaImagenes);

                    // Generar un nombre único para el archivo
                    string nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(imagen.FileName)}";
                    string rutaCompleta = Path.Combine(carpetaImagenes, nombreArchivo);

                    // Guardar el archivo en el sistema de archivos
                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }

                    // Guardar solo el nombre del archivo en la base de datos
                    contact.RutaImagen = nombreArchivo;
                }

                _context.Contact.Add(contact);

                int result = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                log4net.Error("Error al crear el Contacto: ", ex);
                throw;
            }

            return contact;
        }

        public async Task<IEnumerable<Contact>> Filter(int id, string? email, string? telefono, string? direccion)
        {
            IQueryable<Contact> query = _context.Contact;

            if (id > 0)
            {
                query = query.Where(c => c.Id == id);
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(c => EF.Functions.Like(c.Email, $"%{email}%"));
            }

            if (!string.IsNullOrWhiteSpace(telefono))
            {
                query = query.Where(c => EF.Functions.Like(c.Telefono, $"%{telefono}%"));
            }

            if (!string.IsNullOrWhiteSpace(direccion))
            {
                query = query.Where(c => EF.Functions.Like(c.Direccion, $"%{direccion}%"));
            }

            var contacts = await query.ToListAsync();

            return contacts;
        }

        public (Stream Imagen, string TipoContenido)? GetImage(string nombreArchivo)
        {
            string rutaImagen = Path.Combine(_env.WebRootPath, "contact", nombreArchivo);

            if (!System.IO.File.Exists(rutaImagen))
                return null;

            var imagen = System.IO.File.OpenRead(rutaImagen);
            string tipoContenido = ObtenerTipoContenido(Path.GetExtension(nombreArchivo));

            return (imagen, tipoContenido);
        }

        private string ObtenerTipoContenido(string extension)
        {
            return extension.ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}
