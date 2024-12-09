using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Dtos
{
    public class NewPorductDto
    {
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public double DiscountPercentage { get; set; }
        public int Stock { get; set; }
        public int StockMin { get; set; }
    }
}