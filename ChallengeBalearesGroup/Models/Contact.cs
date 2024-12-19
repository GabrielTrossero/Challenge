using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeBalearesGroup.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo 'Nombre' es obligatorio")]
        [StringLength(100, ErrorMessage = "El 'Nombre' no puede tener más de 100 caracteres")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El campo 'Empresa' es obligatorio")]
        [StringLength(100, ErrorMessage = "La 'Empresa' no puede tener más de 100 caracteres")]
        public string? Empresa { get; set; }

        [Required(ErrorMessage = "El 'Email' es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del 'Email' es inválido")]
        [StringLength(100, ErrorMessage = "El 'Email' no puede tener más de 100 caracteres")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La 'Fecha de Nacimiento' es obligatoria")]
        [DataType(DataType.Date, ErrorMessage = "El formato de la 'Fecha de Nacimiento' es inválido")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El 'Teléfono' es obligatorio")]
        [Phone(ErrorMessage = "El formato del 'Teléfono' es inválido")]
        [StringLength(20, ErrorMessage = "El 'Teléfono' no puede tener más de 20 caracteres")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La 'Dirección' es obligatoria")]
        [StringLength(200, ErrorMessage = "La 'Dirección' no puede tener más de 200 caracteres")]
        public string? Direccion { get; set; }

        public string? RutaImagen { get; set; }
    }
}
