using System.ComponentModel.DataAnnotations;

namespace APITaqueriaIguala.Models
{
    public class LoginModel
    {
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
