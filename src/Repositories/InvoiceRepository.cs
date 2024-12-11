using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Interfaces;
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

        public InvoiceRepository(DataContext context, UserManager<AppUser> userManager, IInvoiceItem invoiceItemRepository, ISaleItem saleItemRepository)
        {
            _context = context;
            _userManager = userManager;
            _invoiceItemRepository = invoiceItemRepository;
            _saleItemRepository = saleItemRepository;
        }
    
        public async Task<Invoice> GenerateInvoice(ClientDTO client)
        {
            if (string.IsNullOrWhiteSpace(client.Email)) throw new ArgumentException("Client email is required.");

            var existingUser = await _userManager.FindByEmailAsync(client.Email);
            var newclient = existingUser ?? new AppUser
            {
                UserName = client.Email,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Name = client.Name,
                Rut = client.Rut
            };

            if (existingUser == null)
            {
                var result = await _userManager.CreateAsync(newclient);
                if (!result.Succeeded)
                    throw new Exception("Failed to create user.");

                await _userManager.AddToRoleAsync(newclient, "Client");
            }

            var invoiceState = _context.InvoiceStates.FirstOrDefault(i => i.Name == "Pendiente") ?? throw new KeyNotFoundException("Invoice state 'Pendiente' not found.");

            var paymentMethod = _context.PaymentMethods.FirstOrDefault(p => p.Id == client.PaymentMethodId) ?? throw new KeyNotFoundException($"Payment method with ID {client.PaymentMethodId} not found.");

            var invoice = new Invoice
            {
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

            var saleItems = await _saleItemRepository.CreateSaleItem(invoice.Id, items.Select(i => i.Id).ToList());

            invoice.SaleItems = saleItems;
            invoice.PriceWithoutVAT = saleItems.Sum(s => s.UnitPrice);
            invoice.TotalVAT = invoice.PriceWithoutVAT * 0.19;
            invoice.FinalPrice = invoice.PriceWithoutVAT + invoice.TotalVAT;

            return invoice;
        }


        public async Task<IEnumerable<Invoice>> GetInvoices()
        {
            return await _context.Invoices
                .Include(i => i.User)
                .Include(i => i.InvoiceState)
                .Include(i => i.PaymentMethod)
                .ToListAsync();
        }

        public Task<Invoice> UpdateInvoice(int id, Invoice invoice)
        {
            throw new NotImplementedException();
        }
    }
}