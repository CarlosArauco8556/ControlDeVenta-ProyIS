using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ControlDeVenta_Proy.src.Dtos
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters.")]
        public string Password { get; set; } = null!;
    }
}