
namespace ControlDeVenta_Proy.src.Dtos
{
    public class InvoiceDto
    {
        public int Factura { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public double PriceWithoutVAT { get; set; }
        public double TotalVAT { get; set; }
        public double FinalPrice { get; set; } 
        public List<SaleItemDto> SaleItems { get; set; } = new List<SaleItemDto>();
    }
}