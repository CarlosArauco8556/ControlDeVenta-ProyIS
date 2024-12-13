
namespace ControlDeVenta_Proy.src.DTOs
{
    public class GetSupplyDto
    {
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int SupplyId { get; set; }
        public string SupplyName { get; set; } = string.Empty;
    }
}