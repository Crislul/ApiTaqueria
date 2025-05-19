using System;
using System.ComponentModel.DataAnnotations;

namespace APITaqueriaIguala.Models
{
    public class BusinessLocation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string BusinessName { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
