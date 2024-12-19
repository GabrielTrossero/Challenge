namespace ChallengeBalearesGroup.Models.DTO
{
    public class ContactDTO
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Empresa { get; set; }
        public string? Email { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string Imagen { get; set; }
        public string? TipoContenidoImagen { get; set; }
    }
}
