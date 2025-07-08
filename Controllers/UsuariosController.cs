using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorRepuestosAPI.Data;
using MotorRepuestosAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace MotorRepuestosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios/existe?email=test@test.com
        [HttpGet("existe")]
        public async Task<ActionResult<bool>> EmailExiste([FromQuery] string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        // GET: api/Usuarios/por-email?email=algo@email.com
        [HttpGet("por-email")]
        public async Task<ActionResult<UsuarioDto>> ObtenerPorEmail([FromQuery] string email)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Email == email)
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    NombreUsuario = u.NombreUsuario,
                    Email = u.Email,
                    Telefono = u.Telefono,
                    FechaRegistro = u.FechaRegistro,
                    Rol = u.Rol
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            return Ok(usuario);
        }

        // POST: api/Usuarios/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDto usuarioDto)
        {
            // Validar si el email ya existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuarioDto.Email))
                return BadRequest("El email ya está registrado");

            // Crear hash de contraseña
            using var hmac = new HMACSHA512();
            var usuario = new Usuario
            {
                NombreUsuario = usuarioDto.NombreUsuario,
                Email = usuarioDto.Email,
                Telefono = usuarioDto.Telefono,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(usuarioDto.Password)),
                PasswordSalt = hmac.Key,
                FechaRegistro = DateTime.UtcNow,
                Rol = "Cliente"
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario registrado exitosamente" });
        }
    }
}
