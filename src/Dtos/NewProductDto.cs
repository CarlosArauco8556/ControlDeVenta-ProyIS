
using System.ComponentModel.DataAnnotations;

namespace ControlDeVenta_Proy.src.Dtos
{
    public class NewProductDto
    {
        [StringLength(64, MinimumLength = 4, ErrorMessage = "The name must be between 4 and 64 characters long.")]
        public string Name { get; set; } = string.Empty;
        [Range(0, 100000000, ErrorMessage = "The price must be a positive number and less than 1000000000.")]
        public double Price { get; set; }
        [Range(0, 99.99, ErrorMessage = "The discount percentage must be a positive number less than 99,99.")]
        public double DiscountPercentage { get; set; }
        [Range(0, 100000, ErrorMessage = "Stock must be a positive integer less than 100,000.")]
        public int Stock { get; set; }
        [Range(0, 100000, ErrorMessage = "The minimum stock must be a positive integer less than 100,000.")]
        public int StockMin { get; set; }
    }
}