using System.ComponentModel.DataAnnotations;

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