using ChallengeBalearesGroup.Models.DTO;
using ChallengeBalearesGroup.Repository;

namespace ChallengeBalearesGroup.Models.Mappers
{
    public class ContactMapper
    {
        public static ContactDTO ToDTO(Contact contact, IContactRepository contactRepository)
        {
            var contactDTO = new ContactDTO
            {
                Id = contact.Id,
                Nombre = contact.Nombre,
                Empresa = contact.Empresa,
                Email = contact.Email,
                FechaNacimiento = contact.FechaNacimiento,
                Telefono = contact.Telefono,
                Direccion = contact.Direccion
            };

            // Verificar si el contacto tiene una ruta de imagen y obtenerla
            if (!string.IsNullOrEmpty(contact.RutaImagen))
            {
                var image = contactRepository.GetImage(contact.RutaImagen);
                if (image.HasValue)
                {
                    // Convertir la imagen a Base64
                    using (var memoryStream = new MemoryStream())
                    {
                        image.Value.Imagen.CopyTo(memoryStream);
                        contactDTO.Imagen = Convert.ToBase64String(memoryStream.ToArray());

                    }
                    contactDTO.TipoContenidoImagen = image.Value.TipoContenido;
                }
            }

            return contactDTO;
        }

        public static IEnumerable<ContactDTO> ToDTOList(IEnumerable<Contact> contacts, IContactRepository contactRepository)
        {
            return contacts.Select(contact => ToDTO(contact, contactRepository));
        }


        /*
        public static ContactDTO ToDTO(Contact contact, IContactRepository contactRepository)
        {
            var contactDTO = new ContactDTO
            {
                Id = contact.Id,
                Nombre = contact.Nombre,
                Empresa = contact.Empresa,
                Email = contact.Email,
                FechaNacimiento = contact.FechaNacimiento,
                Telefono = contact.Telefono,
                Direccion = contact.Direccion
            };

            // Verificar si el contacto tiene una ruta de imagen y obtenerla
            if (!string.IsNullOrEmpty(contact.RutaImagen))
            {
                var image = contactRepository.GetImage(contact.RutaImagen);
                if (image.HasValue)
                {
                    // Asignar la imagen y su tipo de contenido al DTO
                    contactDTO.Imagen = image.Value.Imagen;
                    contactDTO.TipoContenidoImagen = image.Value.TipoContenido;
                }
            }

            return contactDTO;
        }*/
    }
}
