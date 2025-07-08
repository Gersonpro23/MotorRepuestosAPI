using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorRepuestosAPI.Data;
using MotorRepuestosAPI.Models;
using MotorRepuestosAPI.Models.DTOs;

[Route("api/[controller]")]
[ApiController]
public class ProformasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProformasController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/Proformas
    [HttpPost]
    public async Task<IActionResult> PostProforma([FromBody] ProformaDto dto)
    {
        var proforma = new Proforma
        {
            UserId = dto.UserId,
            ClienteNombre = dto.ClienteNombre,
            FechaCreacion = DateTime.UtcNow,
            Total = dto.Total,
            Detalles = dto.Detalles.Select(d => new DetalleProforma
            {
                ProductoId = d.ProductoId,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList()
        };

        _context.Proformas.Add(proforma);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Cotización guardada exitosamente" });
    }

    // GET: api/Proformas/usuario/5
    [HttpGet("usuario/{userId}")]
    public async Task<ActionResult<IEnumerable<ProformaDto>>> GetProformasPorUsuario(int userId)
    {
        var proformas = await _context.Proformas
            .Where(p => p.UserId == userId)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
            .ToListAsync();

        var resultado = proformas.Select(p => new ProformaDto
        {
            Id = p.Id,
            UserId = p.UserId ?? 0,
            ClienteNombre = p.ClienteNombre,
            FechaCreacion = p.FechaCreacion,
            Total = p.Total,
            Detalles = p.Detalles.Select(d => new DetalleProformaDto
            {
                ProductoId = d.ProductoId,
                ProductoNombre = d.Producto.Nombre,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Total = d.Cantidad * d.PrecioUnitario
            }).ToList()
        });

        return Ok(resultado);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProforma(int id)
    {
        var proforma = await _context.Proformas
            .Include(p => p.Detalles)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (proforma == null)
            return NotFound();

        _context.DetallesProforma.RemoveRange(proforma.Detalles);
        _context.Proformas.Remove(proforma);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
