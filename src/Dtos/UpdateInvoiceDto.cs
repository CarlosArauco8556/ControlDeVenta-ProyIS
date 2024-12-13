
namespace ControlDeVenta_Proy.src.Dtos
{
    public class UpdateInvoiceDto
    {
        public string Description { get; set; } = null!;
        public int? PaymentMethodId { get; set; }
        public int? InvoiceStateId { get; set; }
    }
}