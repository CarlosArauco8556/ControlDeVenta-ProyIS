using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlDeVenta_Proy.src.Repositories
{
    public class SaleItemRepository : ISaleItem
    {
        private readonly DataContext _context;
        private readonly IInvoiceItem _invoiceItemRepository;

        public SaleItemRepository(DataContext context, IInvoiceItem invoiceItemRepository)
        {
            _context = context;
            _invoiceItemRepository = invoiceItemRepository;
        }

        public async Task<List<SaleItem>> CreateSaleItem(int invoiceId, List<int> productIds)
        {
            if (productIds == null || !productIds.Any())
                throw new ArgumentException("No product IDs provided", nameof(productIds));

            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == invoiceId)
                ?? throw new InvalidOperationException("Invoice not found");

            var productsInDb = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            if (productsInDb.Count != productIds.Count)
                throw new InvalidOperationException("One or more products not found");

            var saleItems = productsInDb.Select(product => new SaleItem
            {
                ProductId = product.Id,
                InvoiceId = invoice.Id,
                Quantity = 1, 
                UnitPrice = product.Price,
            }).ToList();
            
            await _context.SaleItems.AddRangeAsync(saleItems);
            await _context.SaveChangesAsync();

            return saleItems;
        }


        public Task<List<SaleItem>> GetSaleItems()
        {
            throw new NotImplementedException();
        }
    }
}