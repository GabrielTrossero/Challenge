using ChallengeBalearesGroup.Models;
using ChallengeBalearesGroup.Repository;
using System.Linq.Expressions;
using Volo.Abp;

namespace ChallengeBalearesGroup.Services
{
    public interface IContactService
    {
        Task<Contact> Create(Contact contact, IFormFile? imagen);
        Task<Contact> Update(Contact contact, IFormFile? imagen, List<string> errores);
        Task<IEnumerable<ContactDTO>> Filter(int? id, string? email, string? telefono, string? direccion);
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


        public async Task<Contact> Update(Contact contact, IFormFile? imagen, List<string> errores)
        {
            var contactos = await this.contactRepository.Filter(contact.Id, null, null, null);

            // Obtener el contato anterior
            var contactOld = contactos.FirstOrDefault();

            if (contactOld == null)
            {
                errores.Add($"Contacto no encontrado.");
                return null;
            }

            // Actualiza contacto
            contactOld.Nombre = contact.Nombre;
            contactOld.Empresa = contact.Empresa;
            contactOld.Email = contact.Email;
            contactOld.FechaNacimiento = contact.FechaNacimiento;
            contactOld.Telefono = contact.Telefono;
            contactOld.Direccion = contact.Direccion;

            return await this.contactRepository.Update(contactOld, imagen);
        }


        public async Task<IEnumerable<ContactDTO>> Filter(int? id, string? email, string? telefono, string? direccion)
        {
            IEnumerable<Contact> contacts = await this.contactRepository.Filter(id, email, telefono, direccion);

            // Usar el ContactMapper para transformar los contactos en DTOs
            var contactDTOs = ContactMapper.ToDTOList(contacts, this.contactRepository);

            return contactDTOs;
        }
        

        public async Task<IEnumerable<ContactDTO>> GetAllOrderByMail(int id, string email, string telefono, string direccion)
        {
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
