using System.ComponentModel.DataAnnotations;

namespace APITaqueriaIguala.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  // Relación con usuario

        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
