using APITaqueriaIguala.Data;
using APITaqueriaIguala.Models;
using Microsoft.EntityFrameworkCore;

namespace APITaqueriaIguala.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                throw new Exception("Producto no encontrado");
            }
            return product;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        // Obtener productos por ID de categoría
        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            // Obtener productos por el ID de la categoría
            var products = await _context.Products
                                          .Where(p => p.CategoryId == categoryId) // Filtramos por el ID de la categoría
                                          .ToListAsync();

            return products;
        }
    }
}
