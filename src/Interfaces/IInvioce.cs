using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface IInvioce
    {
        Task<IEnumerable<InvoiceDto>> GetInvoices();
        Task<InvoiceDto> GenerateInvoice(ClientDTO client);
        Task<InvoiceDto?> UpdateInvoiceItem(int invoiceId, int productId, int? newProductId, int? quantity, bool? isAddition);
        Task UpdateProduct(SaleItem saleItem, int newProductId);
        Task UpdateInvoiceStatus(int invoiceId);
        Task DeleteInvoice(int invoiceId);
    }
}