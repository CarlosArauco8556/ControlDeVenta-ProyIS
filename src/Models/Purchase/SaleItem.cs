
namespace ControlDeVenta_Proy.src.Models
{
    public class SaleItem
    {
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }
}