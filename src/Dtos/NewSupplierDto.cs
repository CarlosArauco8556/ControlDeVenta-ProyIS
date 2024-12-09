
using System.ComponentModel.DataAnnotations;

namespace ControlDeVenta_Proy.src.DTOs
{
    public class NewSupplierDto
    {
        [Required]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Supplier name must be between 2 and 40 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Rut { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public List<string> ProductNames { get; set; } = [];
    }
}