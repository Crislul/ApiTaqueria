using Newtonsoft.Json;

namespace APITaqueriaIguala.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Slug { get; set; }

        public required string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }  // Coincide con el tipo 'int' de la base de datos

        // Devuelve directamente el nombre de la imagen almacenado en la base de datos
        [JsonProperty("image")]
        public string Image { get; set; }  // Solo el nombre de la imagen, sin URL

        public int Stock { get; set; }  // Coincide con el campo 'Stock' de la base de datos
    }
}