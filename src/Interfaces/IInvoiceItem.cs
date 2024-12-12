using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface IInvoiceItem
    {
        Task<Dictionary<int, int>> GetInvoiceItemsFromCookies();

        Task SaveInvoiceItemsToCookies(int productId, int quantity);

        Task UpdateInvoiceItemsToCookies(int productId, int quantity);

        Task RemoveInvoiceItem(int productId);

        Task ClearInvoiceItemsInCookie();
    }
}