
namespace ControlDeVenta_Proy.src.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public double DiscountPercentage { get; set; }
        public int Stock { get; set; }
        public int StockMin { get; set; }
        public List<SaleItem> SaleItems { get; } = new List<SaleItem>();
        public List<Supply> Supplies { get; } = new List<Supply>();
        public List<Supplier> Suppliers { get; } = new List<Supplier>();
    }
}