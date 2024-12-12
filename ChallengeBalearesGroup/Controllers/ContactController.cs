using ChallengeBalearesGroup.Models;
using ChallengeBalearesGroup.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace ChallengeBalearesGroup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService contactService;

        public ContactController(IContactService contactService) 
        {
            this.contactService = contactService;
        }


        [HttpPost("Create")]
        public async Task<Contact> Create([FromForm] Contact contact, IFormFile? imagen)
        {
            return await this.contactService.Create(contact, imagen);
        }


        [HttpPost("Filter")]
        public async Task<IEnumerable<ContactDTO>> Filter(int id, string? email = null, string? telefono = null, string? direccion = null)
        {
            return await this.contactService.Filter(id, email, telefono, direccion);
        }



        [HttpPost("GetAll")]
        public async Task<IEnumerable<ContactDTO>> GetAll()
        {
            return await this.contactService.Filter(0, null, null, null);
        }


        [HttpGet("GetAllGetAllOrderByMail")]
        public async Task<IEnumerable<ContactDTO>> GetAllOrderByMail()
        {
            return await this.contactService.GetAllOrderByMail(0, null, null, null);
        }


        [HttpGet("GetImage")]
        public IActionResult GetImage(string nombreArchivo)
        {
            var resultado = this.contactService.GetImage(nombreArchivo);

            if (resultado == null)
                return NotFound("La imagen no existe.");

            return File(resultado.Value.Imagen, resultado.Value.TipoContenido);
        }
    }
}
