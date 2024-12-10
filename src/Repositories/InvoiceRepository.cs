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

        public InvoiceRepository(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Invoice> GenerateInvoice(ClientDTO client)
        {
            
            var newclient = new AppUser();

            if (_userManager.FindByEmailAsync(client.Email).Result == null)
            {
                newclient = new AppUser
                {
                    UserName = client.Email,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,
                    Name = client.Name,
                    Rut = client.Rut
                };

                await _userManager.CreateAsync(newclient);
                await _userManager.AddToRoleAsync(newclient, "Client");
            }else
            {
                newclient = await _userManager.FindByEmailAsync(client.Email);
            }

            var invoiceState = _context.InvoiceStates.FirstOrDefault(i => i.Name == "Pendiente");

            if (invoiceState == null)
            {
                throw new Exception("Invoice state not found");
            }

            var paymentMethod = _context.PaymentMethods.FirstOrDefault(p => p.Id == client.PaymentMethodId);

            if (paymentMethod == null)
            {
                throw new Exception("Payment method not found");
            }

            var invoice = new Invoice
            {
                Description = client.Description,
                CreationDate = DateTime.Now,
                PriceWithoutVAT = 0,
                TotalVAT = 0,
                FinalPrice = 0,
                UserId = newclient?.Id ?? throw new Exception("User creation failed"),
                User = newclient ?? throw new Exception("User creation failed"),
                InvoiceStateId = invoiceState?.Id ?? 1,
                InvoiceState = invoiceState ?? throw new Exception("Invoice state not found"),
                PaymentMethodId = paymentMethod.Id,
                PaymentMethod = paymentMethod
            };

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

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