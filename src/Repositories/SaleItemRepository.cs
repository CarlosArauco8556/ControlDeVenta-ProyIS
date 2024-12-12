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

        public async Task<List<SaleItem>> CreateSaleItem(int invoiceId, Dictionary<int, int> products)
        {
            var saleItems = new List<SaleItem>();
            var invoiceItems = await _invoiceItemRepository.GetInvoiceItemsFromCookies();

            foreach (var product in products)
            {
                var productDb = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.Key);
                if (productDb != null)
                {
                    var saleItem = new SaleItem
                    {
                        InvoiceId = invoiceId,
                        Invoice = await _context.Invoices.FirstOrDefaultAsync(x => x.Id == invoiceId) ?? new Invoice(),
                        ProductId = product.Key,
                        Product = productDb,
                        Quantity = product.Value,
                        UnitPrice = productDb.Price
                    };
                    saleItems.Add(saleItem);
                }
            }
            
            await _context.SaleItems.AddRangeAsync(saleItems);
            await _context.SaveChangesAsync();
            return saleItems;
        }


        public Task<List<SaleItem>> GetSaleItems()
        {
            return _context.SaleItems.ToListAsync() ?? throw new ArgumentNullException("No sale items found.");
        }
       
    }
}