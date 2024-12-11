using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface IInvoiceItem
    {
        Task<List<Product>> GetInvoiceItemsFromCookies();

        Task SaveInvoiceItemsToCookies(int productId);

        Task RemoveInvoiceItem(int productId);

        Task ClearInvoiceItemsInCookie();
    }
}