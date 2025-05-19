using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APITaqueriaIguala.Data;
using APITaqueriaIguala.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITaqueriaIguala.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessLocationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BusinessLocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todas las ubicaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessLocation>>> GetBusinessLocations()
        {
            return await _context.BusinessLocations.ToListAsync();
        }

        // Obtener una ubicación por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessLocation>> GetBusinessLocation(int id)
        {
            var businessLocation = await _context.BusinessLocations.FindAsync(id);

            if (businessLocation == null)
            {
                return NotFound();
            }

            return businessLocation;
        }

        // Agregar una nueva ubicación
        [HttpPost]
        public async Task<ActionResult<BusinessLocation>> PostBusinessLocation(BusinessLocation businessLocation)
        {
            businessLocation.CreatedAt = DateTime.UtcNow; // Asignamos la fecha actual
            _context.BusinessLocations.Add(businessLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBusinessLocation), new { id = businessLocation.Id }, businessLocation);
        }

        // Actualizar una ubicación existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusinessLocation(int id, BusinessLocation businessLocation)
        {
            if (id != businessLocation.Id)
            {
                return BadRequest();
            }

            _context.Entry(businessLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.BusinessLocations.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Eliminar una ubicación
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessLocation(int id)
        {
            var businessLocation = await _context.BusinessLocations.FindAsync(id);
            if (businessLocation == null)
            {
                return NotFound();
            }

            _context.BusinessLocations.Remove(businessLocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}