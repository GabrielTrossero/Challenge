using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeBalearesGroup.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Empresa { get; set; }
        public string? Email { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? RutaImagen { get; set; }
    }
}
