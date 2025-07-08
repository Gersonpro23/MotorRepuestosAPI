namespace MotorRepuestosAPI.Models
{
    public class UsuarioRegistroDto
    {
        public required string NombreUsuario { get; set; }
        public required string Email { get; set; }
        public required string Telefono { get; set; }
        public required string Password { get; set; }
    }
}
