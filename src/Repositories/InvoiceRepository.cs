using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Mappers;
using ControlDeVenta_Proy.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ControlDeVenta_Proy.src.Repositories
{
    public class InvoiceRepository : IInvioce
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IInvoiceItem _invoiceItemRepository;
        private readonly ISaleItem _saleItemRepository;
        private readonly IInvoiceCode _invoiceCodeRepository;

        public InvoiceRepository(DataContext context, UserManager<AppUser> userManager, IInvoiceItem invoiceItemRepository, ISaleItem saleItemRepository, IInvoiceCode invoiceCodeRepository)
        {
            _context = context;
            _userManager = userManager;
            _invoiceItemRepository = invoiceItemRepository;
            _saleItemRepository = saleItemRepository;
            _invoiceCodeRepository = invoiceCodeRepository;
        }

        public async Task DeleteInvoice(int invoiceId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.SaleItems)
                .FirstOrDefaultAsync(i => i.Id == invoiceId)
                ?? throw new KeyNotFoundException($"Invoice with ID {invoiceId} not found.");

            _context.SaleItems.RemoveRange(invoice.SaleItems);
            _context.Invoices.Remove(invoice);

            await _context.SaveChangesAsync();
        }

        public async Task<InvoiceDto> GenerateInvoice(ClientDTO client)
        {
            if (string.IsNullOrWhiteSpace(client.Email)) throw new ArgumentException("Client email is required.");

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == client.Email);
            
            var newclient = new AppUser();

            if (existingUser == null)
            {
                newclient = new AppUser
                {
                    UserName = client.Email,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,
                    Name = client.Name,
                    Rut = client.Rut
                };

                var result = await _userManager.CreateAsync(newclient);
                if (!result.Succeeded)
                    throw new Exception("Failed to create user.");

                await _userManager.AddToRoleAsync(newclient, "Client");
            } else 
            {   
                newclient = existingUser;
            }

            var invoiceState = _context.InvoiceStates.FirstOrDefault(i => i.Name == "Pendiente") ?? throw new KeyNotFoundException("Invoice state 'Pendiente' not found.");

            var paymentMethod = _context.PaymentMethods.FirstOrDefault(p => p.Id == client.PaymentMethodId) ?? throw new KeyNotFoundException($"Payment method with ID {client.PaymentMethodId} not found.");

            var invoiceCode = await _invoiceCodeRepository.GenerateCode();

            if (invoiceCode == null)
                throw new InvalidOperationException("Failed to generate invoice code.");

            var invoice = new Invoice
            {
                InvoiceCodeId = invoiceCode.Id,
                InvoiceCode = invoiceCode,
                Description = client.Description,
                CreationDate = DateTime.Now,
                PriceWithoutVAT = 0,
                TotalVAT = 0,
                FinalPrice = 0,
                UserId = newclient.Id,
                InvoiceStateId = invoiceState.Id,
                PaymentMethodId = paymentMethod.Id
            };

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            var items = await _invoiceItemRepository.GetInvoiceItemsFromCookies();

            if (items == null || !items.Any())
                throw new InvalidOperationException("No items in the invoice.");

            var saleItems = await _saleItemRepository.CreateSaleItem(invoice.Id, items);

            invoice.SaleItems = saleItems;
            invoice.PriceWithoutVAT = saleItems.Sum(s => s.UnitPrice);
            invoice.TotalVAT = invoice.PriceWithoutVAT * 0.19;
            invoice.FinalPrice = invoice.PriceWithoutVAT + invoice.TotalVAT;

            await _context.SaveChangesAsync();

            await _invoiceItemRepository.ClearInvoiceItemsInCookie();
            return InvoiceMapper.MapInvoiceToDTO(invoice, InvoiceMapper.ToSaleItemDto(saleItems));
        }


        public async Task<IEnumerable<InvoiceDto>> GetInvoices()
        {
            var invoices = await _context.Invoices
                .Include(i => i.User)
                .Include(i => i.InvoiceState)
                .Include(i => i.PaymentMethod)
                .Include(i => i.InvoiceCode)
                .ToListAsync();
            
            var saleItems = await _context.SaleItems
                .Include(s => s.Product)
                .ToListAsync();
            
            return invoices.Select(i => InvoiceMapper.MapInvoiceToDTO(i, InvoiceMapper.ToSaleItemDto(saleItems.Where(s => s.InvoiceId == i.Id).ToList())));
        }

        public async Task<InvoiceDto?> UpdateInvoiceItem(int invoiceId, int productId, int? newProductId, int? quantity, bool? isAddition)
        {
            var invoice = await _context.Invoices.Include(i => i.SaleItems).FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
                throw new KeyNotFoundException($"Invoice with ID {invoiceId} not found.");
            
            var saleItem = invoice.SaleItems.FirstOrDefault(s => s.ProductId == productId);

            if (saleItem == null)
                throw new KeyNotFoundException($"Sale item with product ID {productId} not found.");
            
            if (isAddition == true)
                saleItem.Quantity += quantity ?? 1;
            else
                saleItem.Quantity -= quantity ?? 1;

            if (newProductId != null)
                await UpdateProduct(saleItem, newProductId.Value);

            invoice.PriceWithoutVAT = invoice.SaleItems.Sum(s => s.UnitPrice * s.Quantity);
            invoice.TotalVAT = invoice.PriceWithoutVAT * 0.19;
            invoice.FinalPrice = invoice.PriceWithoutVAT + invoice.TotalVAT;

            await _context.SaveChangesAsync();

            return InvoiceMapper.MapInvoiceToDTO(invoice, InvoiceMapper.ToSaleItemDto(invoice.SaleItems));
        }

        public async Task UpdateInvoiceStatus(int invoiceId)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Id == invoiceId) ?? throw new KeyNotFoundException($"Invoice with ID {invoiceId} not found.");
            
            if (invoice.InvoiceStateId == 2)
                throw new InvalidOperationException("Invoice already delivered.");

            var invoiceState = _context.InvoiceStates.FirstOrDefault(i => i.Id == 2) ?? throw new KeyNotFoundException("Invoice state 'Entregada' not found.");

            invoice.InvoiceStateId = invoiceState.Id;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(SaleItem saleItem, int newProductId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == newProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {newProductId} not found.");

            _context.SaleItems.Remove(saleItem);
            await _context.SaveChangesAsync();

            var newSaleItem = new SaleItem
            {
                InvoiceId = saleItem.InvoiceId,
                ProductId = newProductId,
                Quantity = saleItem.Quantity,
                UnitPrice = product.Price
            };

            await _context.SaleItems.AddAsync(newSaleItem);
            await _context.SaveChangesAsync();
        }

    }
}