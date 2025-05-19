using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APITaqueriaIguala.Data;
using APITaqueriaIguala.Models;
using APITaqueriaIguala.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace APITaqueriaIguala.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] 
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todos los usuarios con paginación
        [HttpGet]
        public async Task<IActionResult> GetUsuarios(int pageNumber = 1, int pageSize = 10)
        {
            var usuarios = await _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // Obtener un usuario por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(string id)
        {
            var usuario = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            return Ok(usuario);
        }

        // Crear un nuevo usuario
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUsuario([FromBody] UsuarioDto usuarioDto)
        {
            if (usuarioDto == null || string.IsNullOrWhiteSpace(usuarioDto.UserName))
                return BadRequest(new { mensaje = "Datos inválidos" });

            try
            {
                var usuario = new AspNetUsers
                {
                    UserName = usuarioDto.UserName,
                    Email = usuarioDto.Email,
                    PhoneNumber = usuarioDto.PhoneNumber
                };

                _context.Users.Add(usuario);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuarioDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al crear el usuario", error = ex.Message });
            }
        }

        // Actualizar un usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(string id, [FromBody] UsuarioDto usuarioDto)
        {
            if (usuarioDto == null || id != usuarioDto.Id)
                return BadRequest(new { mensaje = "Datos inválidos" });

            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            usuario.UserName = usuarioDto.UserName;
            usuario.Email = usuarioDto.Email;
            usuario.PhoneNumber = usuarioDto.PhoneNumber;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el usuario", error = ex.Message });
            }
        }

        // Eliminar un usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(string id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            try
            {
                _context.Users.Remove(usuario);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar el usuario", error = ex.Message });
            }
        }
    }
}