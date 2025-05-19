using APITaqueriaIguala.Data;
using APITaqueriaIguala.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APITaqueriaIguala.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]  // Requiere autenticación
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener carrito del usuario autenticado
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = await _context.Carts.Include(c => c.Items)
                                           .FirstOrDefaultAsync(c => c.UserId == userId);

            return cart != null ? Ok(cart) : NotFound("Carrito vacío.");
        }

        // Agregar producto al carrito
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItem item)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = await _context.Carts.Include(c => c.Items)
                                           .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                item.CartId = cart.Id;
                _context.CartItems.Add(item);
            }

            await _context.SaveChangesAsync();
            return Ok(cart);
        }

        // Eliminar producto del carrito
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = await _context.Carts.Include(c => c.Items)
                                           .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return NotFound("Carrito no encontrado.");

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item == null) return NotFound("Producto no encontrado.");

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(cart);
        }

        // Vaciar carrito
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = await _context.Carts.Include(c => c.Items)
                                           .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return NotFound("Carrito no encontrado.");

            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
            return Ok("Carrito vaciado.");
        }
    }
}