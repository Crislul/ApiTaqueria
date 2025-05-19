using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APITaqueriaIguala.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Mínimo 2 caracteres!!!")]
        public required string Name { get; set; }

        public required string Slug { get; set; }

        [Required, MinLength(4, ErrorMessage = "Mínimo 4 caracteres!!!")]
        public required string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debería elegir una categoría!!!")]
        public int CategoryId { get; set; }

        [JsonProperty("image")]
        public required string image { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; } // 🛒 Agregado para gestionar inventario
    }
}