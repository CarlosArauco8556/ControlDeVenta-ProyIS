using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlDeVenta_Proy.src.Repositories
{
    public class InvoiceRepository : IInvioce
    {
        private readonly DataContext _context;

        public InvoiceRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Invoice> GenerateInvoice(ClientDTO client)
        {
            
            var newclient = new AppUser();

            if (_context.Users.Any(u => u.Rut == client.Rut))
            {
                newclient = _context.Users.FirstOrDefault(u => u.Rut == client.Rut);
            }
            else
            {
                newclient = new AppUser
                {
                    Name = client.Name,
                    Rut = client.Rut,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,
                };
            }

            if (newclient != null)
            {
                _context.Users.Add(newclient);
            }

            await _context.SaveChangesAsync();

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
                UserId = newclient?.Id ?? "0",
                User = newclient == null || newclient.Id == "0" ? new AppUser() : newclient,
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