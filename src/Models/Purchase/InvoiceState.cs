
namespace ControlDeVenta_Proy.src.Models
{
    public class InvoiceState
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Invoice> Invoices { get; } = new List<Invoice>();
    }
}