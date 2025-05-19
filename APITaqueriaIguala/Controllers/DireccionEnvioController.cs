using APITaqueriaIguala.Data;
using APITaqueriaIguala.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APITaqueriaIguala.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionEnvioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DireccionEnvioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todas las direcciones de un usuario
        [HttpGet]
        public async Task<IActionResult> GetDirecciones([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("El email es requerido.");

            var direcciones = await _context.DireccionesEnvio
                .Where(d => d.Email == email)
                .ToListAsync();

            return Ok(direcciones);
        }

        // Guardar una nueva dirección
        [HttpPost]
        public async Task<IActionResult> GuardarDireccionEnvio([FromBody] DireccionEnvio nuevaDireccion)
        {
            if (nuevaDireccion == null)
                return BadRequest("La dirección de envío no es válida.");

            _context.DireccionesEnvio.Add(nuevaDireccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDirecciones), new { email = nuevaDireccion.Email }, nuevaDireccion);
        }

        // Editar una dirección
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarDireccionEnvio(int id, [FromBody] DireccionEnvio direccionActualizada)
        {
            if (direccionActualizada == null || direccionActualizada.Id != id)
                return BadRequest("La dirección de envío no es válida.");

            var direccionExistente = await _context.DireccionesEnvio.FindAsync(id);
            if (direccionExistente == null)
                return NotFound();

            direccionExistente.Calle = direccionActualizada.Calle;
            direccionExistente.Colonia = direccionActualizada.Colonia;
            direccionExistente.Ciudad = direccionActualizada.Ciudad;
            direccionExistente.CodigoPostal = direccionActualizada.CodigoPostal;
            direccionExistente.Telefono = direccionActualizada.Telefono;
            direccionExistente.Referencias = direccionActualizada.Referencias;

            await _context.SaveChangesAsync();

            return Ok(direccionExistente);
        }

        // Eliminar una dirección
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarDireccionEnvio(int id)
        {
            var direccion = await _context.DireccionesEnvio.FindAsync(id);
            if (direccion == null)
                return NotFound();

            _context.DireccionesEnvio.Remove(direccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Seleccionar una dirección
        [HttpPost("{id}/seleccionar")]
        public async Task<IActionResult> SeleccionarDireccionEnvio(int id, [FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("El email es requerido.");

            var direcciones = await _context.DireccionesEnvio
                .Where(d => d.Email == email)
                .ToListAsync();

            foreach (var d in direcciones)
                d.EsSeleccionada = false;

            var direccionSeleccionada = direcciones.FirstOrDefault(d => d.Id == id);
            if (direccionSeleccionada != null)
            {
                direccionSeleccionada.EsSeleccionada = true;
                await _context.SaveChangesAsync();
            }

            return Ok(direccionSeleccionada);
        }

        // GET: api/DireccionEnvio/seleccionada?email=correo@ejemplo.com
        [HttpGet("seleccionada")]
        public async Task<IActionResult> ObtenerDireccionSeleccionada([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("El email es requerido.");

            var direccion = await _context.DireccionesEnvio
                .FirstOrDefaultAsync(d => d.Email == email && d.EsSeleccionada);

            if (direccion == null)
                return NotFound("No hay dirección seleccionada.");

            return Ok(direccion);
        }
    }
}