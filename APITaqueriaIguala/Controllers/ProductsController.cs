using APITaqueriaIguala.DTOs;
using APITaqueriaIguala.Models;
using APITaqueriaIguala.Services;
using Microsoft.AspNetCore.Mvc;

namespace APITaqueriaIguala.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();

            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                Image = p.image, // Solo asigna el nombre de la imagen, no la URL completa
                Stock = p.Stock
            }).ToList();

            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var newProduct = await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest();
            var updated = await _productService.UpdateProductAsync(product);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // Obtener productos por nombre de categoría
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            // Obtener los productos por ID de categoría
            var products = await _productService.GetProductsByCategoryAsync(categoryId);

            if (products == null || !products.Any())
                return NotFound("No se encontraron productos para esta categoría.");

            // Mapear los productos a ProductDto
            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                Image = p.image, // 🔹 Ahora solo devuelve el valor exacto de la BD, sin modificarlo
                Stock = p.Stock
            }).ToList();

            return Ok(productDtos);
        }
    }
}