using ChallengeBalearesGroup.Models;
using ChallengeBalearesGroup.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChallengeBalearesGroup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService) 
        {
            _contactService = contactService;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Contact contact, IFormFile? imagen)
        {
            var errores = new List<string>();
            var result = await _contactService.Create(contact, imagen, errores);

            if (errores.Any())
            {
                return BadRequest(new { Errores = errores });
            }

            return Ok(result);
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] Contact contact, IFormFile? imagen)
        {
            var errores = new List<string>();
            var result = await _contactService.Update(contact, imagen, errores);

            if (errores.Any())
            {
                return BadRequest(new { Errores = errores });
            }

            return Ok(result);
        }


        [HttpGet("Filter")]
        public async Task<IEnumerable<ContactDTO>> Filter(int id, string? email = null, string? telefono = null, string? direccion = null)
        {
            return await _contactService.Filter(id, email, telefono, direccion);
        }


        [HttpGet("GetAll")]
        public async Task<IEnumerable<ContactDTO>> GetAll()
        {
            return await _contactService.Filter(0, null, null, null);
        }


        [HttpGet("GetAllGetAllOrderByMail")]
        public async Task<IEnumerable<ContactDTO>> GetAllOrderByMail()
        {
            return await _contactService.GetAllOrderByMail(0, null, null, null);
        }


        [HttpGet("GetImage")]
        public IActionResult GetImage(string nombreArchivo)
        {
            var resultado = _contactService.GetImage(nombreArchivo);

            if (resultado == null)
                return NotFound("La imagen no existe.");

            return File(resultado.Value.Imagen, resultado.Value.TipoContenido);
        }
    }
}
