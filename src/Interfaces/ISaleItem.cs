using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface ISaleItem
    {
        Task<List<SaleItem>> CreateSaleItem(int invoiceId, Dictionary<int, int> products);
        Task<List<SaleItem>> GetSaleItems();
    }
}