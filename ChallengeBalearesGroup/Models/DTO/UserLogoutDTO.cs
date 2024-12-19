using System.ComponentModel.DataAnnotations;

namespace ChallengeBalearesGroup.Models.DTO
{
    public class UserLogoutDTO : IValidatableObject
    {
        [StringLength(100, ErrorMessage = "El 'Username' no puede tener más de 100 caracteres.")]
        public string? Username { get; set; }

        [EmailAddress(ErrorMessage = "El 'Correo' no tiene un formato válido.")]
        [StringLength(100, ErrorMessage = "El 'Correo' no puede tener más de 100 caracteres.")]
        public string? Correo { get; set; }


        // Validación personalizada
        // Si username o correo son null, lanza error
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Correo))
            {
                yield return new ValidationResult(
                    "Especificar el 'Username' o el 'Correo'.",
                    new[] { nameof(Username), nameof(Correo) }
                );
            }
        }
    }
}
