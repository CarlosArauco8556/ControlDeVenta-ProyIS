using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControlDeVenta_Proy.src.Dtos
{
    public class SaleItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double DiscountPercentage { get; set; }
        public double Total { get; set; }
    }
}