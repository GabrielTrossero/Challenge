using System.ComponentModel.DataAnnotations;

namespace ChallengeBalearesGroup.Models.DTO
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "El 'Username' es obligatorio.")]
        [StringLength(100, ErrorMessage = "El 'Username' no puede tener más de 100 caracteres.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "El 'Correo' es obligatorio.")]
        [EmailAddress(ErrorMessage = "El 'Correo' no tiene un formato válido.")]
        [StringLength(100, ErrorMessage = "El 'Correo' no puede tener más de 100 caracteres.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La 'Password' es obligatoria.")]
        [MinLength(8, ErrorMessage = "La 'Password' debe tener al menos 8 caracteres.")]
        [StringLength(500, ErrorMessage = "La 'Password' no puede tener más de 500 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El 'Nombre' es obligatorio.")]
        [StringLength(100, ErrorMessage = "El 'Nombre' no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El 'Apellido' es obligatorio.")]
        [StringLength(100, ErrorMessage = "El 'Apellido' no puede tener más de 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;
    }

}
