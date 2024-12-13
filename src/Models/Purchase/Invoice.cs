
namespace ControlDeVenta_Proy.src.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public int InvoiceStateId { get; set; }
        public InvoiceState InvoiceState { get; set; } = null!;
        public List<SaleItem> SaleItems { get; } = new List<SaleItem>();
        public double PriceWithoutVAT { get; set; }
        public double TotalVAT { get; set; }
        public double FinalPrice { get; set; } 
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = new PaymentMethod();
    }
}