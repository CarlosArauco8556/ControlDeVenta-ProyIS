using Microsoft.AspNetCore.Identity;

namespace ControlDeVenta_Proy.src.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Rut { get; set; } = string.Empty;
    }
}