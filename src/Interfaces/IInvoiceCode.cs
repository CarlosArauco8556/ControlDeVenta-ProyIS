using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Models.Purchase;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface IInvoiceCode
    {
        Task<InvoiceCode> GenerateCode();
    }
}