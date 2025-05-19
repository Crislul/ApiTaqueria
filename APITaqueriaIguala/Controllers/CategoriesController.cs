using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APITaqueriaIguala.Data;
using APITaqueriaIguala.Models;
using System.Linq;
using System.Threading.Tasks;

namespace APITaqueriaIguala.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todas las categorías
        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            return Ok(await _context.Categories.ToListAsync());
        }

        // Obtener una categoría por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoria(int id)
        {
            var categoria = await _context.Categories.FindAsync(id);
            if (categoria == null)
                return NotFound();
            return Ok(categoria);
        }

        // Crear una nueva categoría
        [HttpPost]
        public async Task<IActionResult> CreateCategoria([FromBody] Categories categoria)
        {
            if (categoria == null)
                return BadRequest();

            _context.Categories.Add(categoria);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Id }, categoria);
        }

        // Actualizar una categoría
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] Categories categoria)
        {
            if (id != categoria.Id)
                return BadRequest();

            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Eliminar una categoría
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categories.FindAsync(id);
            if (categoria == null)
                return NotFound();

            _context.Categories.Remove(categoria);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
