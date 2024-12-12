using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Models.Purchase;

namespace ControlDeVenta_Proy.src.Repositories
{
    public class InvoiceCodeRepository : IInvoiceCode
    {
        private readonly DataContext _context;
        
        public InvoiceCodeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<InvoiceCode> GenerateCode()
        {
            var invoiceCode = new InvoiceCode
            {
                Code = Guid.NewGuid().ToString()
            };

            await _context.InvoiceCodes.AddAsync(invoiceCode);
            await _context.SaveChangesAsync();

            return invoiceCode;
        }
    }
}