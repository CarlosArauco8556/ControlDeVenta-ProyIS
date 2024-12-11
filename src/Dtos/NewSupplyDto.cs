
using System.ComponentModel.DataAnnotations;

namespace ControlDeVenta_Proy.src.DTOs
{
    public class NewSupplyDto
    {
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int SupplierId { get; set; }
    }
}