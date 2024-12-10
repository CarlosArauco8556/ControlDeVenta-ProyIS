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
        Task<IEnumerable<Invoice>> GetInvoices();
        Task<Invoice> GenerateInvoice(ClientDTO client);
        Task<Invoice> UpdateInvoice(int id, Invoice invoice);
    }
}