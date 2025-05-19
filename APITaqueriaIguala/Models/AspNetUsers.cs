using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APITaqueriaIguala.Models
{
    public class AspNetUsers : IdentityUser
    {
        public string Occupation { get; set; }  // Puedes agregar otras propiedades que no estén en IdentityUser
    }
}
