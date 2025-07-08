using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorRepuestosAPI.Data;
using MotorRepuestosAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaMotor>>> GetCategorias()
    {
        return await _context.CategoriasMotores.ToListAsync();
    }

    // GET: api/Categorias/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaMotor>> GetCategoria(int id)
    {
        var categoria = await _context.CategoriasMotores.FindAsync(id);

        if (categoria == null)
        {
            return NotFound();
        }

        return categoria;
    }

    // POST: api/Categorias
    [HttpPost]
    public async Task<ActionResult<CategoriaMotor>> PostCategoria(CategoriaMotor categoria)
    {
        _context.CategoriasMotores.Add(categoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
    }
}