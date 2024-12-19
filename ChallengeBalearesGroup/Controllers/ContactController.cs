using ChallengeBalearesGroup.Models;
using ChallengeBalearesGroup.Models.DTO;
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
    /// <summary>
    /// Controlador para gestionar operaciones relacionadas con contactos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;


        /// <summary>
        /// Constructor para inicializar el servicio de contacto.
        /// </summary>
        /// <param name="contactService">Instancia del servicio de contacto.</param>
        public ContactController(IContactService contactService) 
        {
            _contactService = contactService;
        }


        /// <summary>
        /// Crea un nuevo contacto.
        /// </summary>
        /// <param name="contact">Datos del contacto a crear.</param>
        /// <param name="imagen">Archivo de imagen opcional.</param>
        /// <returns>Resultado de la operación de creación.</returns>
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


        /// <summary>
        /// Actualiza un contacto existente.
        /// </summary>
        /// <param name="contact">Datos actualizados del contacto.</param>
        /// <param name="imagen">Archivo de imagen opcional.</param>
        /// <returns>Resultado de la operación de actualización.</returns>
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


        /// <summary>
        /// Filtra los contactos según criterios opcionales.
        /// </summary>
        /// <param name="id">ID del contacto.</param>
        /// <param name="email">Correo electrónico opcional.</param>
        /// <param name="telefono">Teléfono opcional.</param>
        /// <param name="direccion">Dirección opcional.</param>
        /// <returns>Lista de contactos que cumplen con los criterios especificados.</returns>
        [HttpGet("Filter")]
        public async Task<IEnumerable<ContactDTO>> Filter(int id, string? email = null, string? telefono = null, string? direccion = null)
        {
            return await _contactService.Filter(id, email, telefono, direccion);
        }


        /// <summary>
        /// Obtiene todos los contactos registrados.
        /// </summary>
        /// <returns>Lista de todos los contactos.</returns>
        [HttpGet("GetAll")]
        public async Task<IEnumerable<ContactDTO>> GetAll()
        {
            return await _contactService.Filter(0, null, null, null);
        }


        /// <summary>
        /// Obtiene todos los contactos ordenados por correo electrónico.
        /// </summary>
        /// <returns>Lista de contactos ordenada por correo electrónico.</returns>
        [HttpGet("GetAllGetAllOrderByMail")]
        public async Task<IEnumerable<ContactDTO>> GetAllOrderByMail()
        {
            return await _contactService.GetAllOrderByMail(0, null, null, null);
        }


        /// <summary>
        /// Obtiene la imagen asociada a un contacto.
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo de imagen.</param>
        /// <returns>Archivo de imagen si existe; mensaje de error en caso contrario.</returns>
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
