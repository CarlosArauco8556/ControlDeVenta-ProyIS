using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlDeVenta_Proy.src.Dtos
{
    public class AppUserUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Rut { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}