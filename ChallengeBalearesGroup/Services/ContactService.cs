using ChallengeBalearesGroup.Models;
using ChallengeBalearesGroup.Repository;
using System.Linq.Expressions;

namespace ChallengeBalearesGroup.Services
{
    public interface IContactService
    {
        Task<Contact> Create(Contact contact, IFormFile? imagen);
        Task<IEnumerable<ContactDTO>> Filter(int id, string email, string telefono, string direccion);
        (Stream Imagen, string TipoContenido)? GetImage(string nombreArchivo);
        Task<IEnumerable<ContactDTO>> GetAllOrderByMail(int id, string email, string telefono, string direccion);
    }

    public class ContactService : IContactService
    {
        private readonly IContactRepository contactRepository;


        public ContactService(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }


        public async Task<Contact> Create(Contact contact, IFormFile? imagen)
        {
            return await this.contactRepository.Create(contact, imagen);
        }


        public async Task<IEnumerable<ContactDTO>> Filter(int id, string email, string telefono, string direccion)
        {
            IEnumerable<Contact> contacts = await this.contactRepository.Filter(id, email, telefono, direccion);

            // Usar el ContactMapper para transformar los contactos en DTOs
            var contactDTOs = ContactMapper.ToDTOList(contacts, this.contactRepository);

            return contactDTOs;
        }
        
        public async Task<IEnumerable<ContactDTO>> GetAllOrderByMail(int id, string email, string telefono, string direccion)
        {
            // Ordenar por Email
            var resultByEmail = await OrderBy(id, email, telefono, direccion, c => c.Id);

            return resultByEmail;
        }

        public async Task<IEnumerable<ContactDTO>> OrderBy<TOrderKey>(
            int id,
            string email,
            string telefono,
            string direccion,
            Expression<Func<Contact, TOrderKey>> orderBy)
        {
            // Obtener la lista de contactos desde el repositorio
            IEnumerable<Contact> contacts = await this.contactRepository.Filter(id, email, telefono, direccion);

            // Ordenar usando la expresión lambda proporcionada
            var orderedContacts = contacts.AsQueryable().OrderBy(orderBy);

            // Usar el ContactMapper para transformar los contactos en DTOs
            var contactDTOs = ContactMapper.ToDTOList(orderedContacts, this.contactRepository);

            return contactDTOs;
        }

        public (Stream Imagen, string TipoContenido)? GetImage(string nombreArchivo)
        {
            return this.contactRepository.GetImage(nombreArchivo);
        }
    }
}
