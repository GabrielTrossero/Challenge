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
        Task<Contact> Update(Contact contact, IFormFile? imagen);
        Task<IEnumerable<Contact>> Filter(int? id, string? email, string? telefono, string? direccion);
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
                    // Guardar imagen y obtener el nombre del archivo
                    contact.RutaImagen = await SaveImageAsync(imagen, "contact");
                }

                _context.Contact.Add(contact);
                await _context.SaveChangesAsync();

                return contact;
            }
            catch (Exception ex)
            {
                log4net.Error("Error al crear el Contacto: ", ex);
                throw;
            }
        }


        public async Task<Contact> Update(Contact newContact, IFormFile? imagen)
        {
            try
            {
                if (imagen != null)
                {
                    // Guardar nueva imagen
                    string nuevaImagen = await SaveImageAsync(imagen, "contact");

                    // Eliminar imagen anterior si existe
                    await DeleteExistingImageAsync(newContact.RutaImagen, "contact");

                    // Actualizar la ruta de la imagen
                    newContact.RutaImagen = nuevaImagen;
                }

                _context.Contact.Update(newContact);
                await _context.SaveChangesAsync();

                return newContact;
            }
            catch (Exception ex)
            {
                log4net.Error("Error al actualizar el contacto: ", ex);
                throw;
            }
        }


        public async Task<IEnumerable<Contact>> Filter(int? id, string? email, string? telefono, string? direccion)
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


        private async Task<string> SaveImageAsync(IFormFile imagen, string folderName)
        {
            try
            {
                // Definir la ruta de la carpeta
                string carpetaImagenes = Path.Combine(_env.WebRootPath, folderName);

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

                return nombreArchivo;
            }
            catch (Exception ex)
            {
                log4net.Error("Error al guardar la imagen: ", ex);
                throw;
            }
        }


        private async Task DeleteExistingImageAsync(string? rutaImagen, string folderName)
        {
            if (!string.IsNullOrEmpty(rutaImagen))
            {
                try
                {
                    string carpetaImagenes = Path.Combine(_env.WebRootPath, folderName);
                    string rutaCompleta = Path.Combine(carpetaImagenes, rutaImagen);

                    if (File.Exists(rutaCompleta))
                    {
                        File.Delete(rutaCompleta);
                        await Task.CompletedTask;  // Simular operación asincrónica
                    }
                }
                catch (Exception ex)
                {
                    log4net.Error("Error al eliminar la imagen anterior del contacto: ", ex);
                }
            }
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
