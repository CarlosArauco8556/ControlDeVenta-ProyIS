using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ControlDeVenta_Proy.src.Services
{
    public class InvoiceItemsService : IInvoiceItem
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public InvoiceItemsService(IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }


        public Task ClearInvoiceItemsInCookie()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Response.Cookies.Delete("InvoiceItem");
            }
            return Task.CompletedTask;
        }

        public Task<Dictionary<int, int>> GetInvoiceItemsFromCookies()
        {
            var cartItems = new Dictionary<int, int>();
            var context = _httpContextAccessor.HttpContext;

            if (context != null)
            {
                var cartCookie = context.Request.Cookies["InvoiceItem"];
                if (!string.IsNullOrEmpty(cartCookie))
                {
                    try
                    {

                        cartItems = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartCookie);
                    }
                    catch (JsonSerializationException ex)
                    {
                        Console.WriteLine($"Error during deserialization: {ex.Message}");
                    }
                }
            }
            return Task.FromResult(cartItems ?? new Dictionary<int, int>());
        }


        public Task RemoveInvoiceItem(int productId)
        {
            var context = _httpContextAccessor.HttpContext;
            var invoiceItems = GetInvoiceItemsFromCookies().Result;
            if (invoiceItems.ContainsKey(productId))
            {
                invoiceItems.Remove(productId);
            }
            if (context != null)
            {
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                };
                context.Response.Cookies.Append("InvoiceItem", JsonConvert.SerializeObject(invoiceItems), options);
            }
            return Task.CompletedTask;
        }

        public async Task SaveInvoiceItemsToCookies(int productId, int quantity)
        {
            var context = _httpContextAccessor.HttpContext;
            var invoiceItems = GetInvoiceItemsFromCookies().Result;

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product != null)
            {
                if (invoiceItems.ContainsKey(productId))
                {
                    invoiceItems[productId] += quantity;
                }
                else
                {
                    invoiceItems.Add(productId, quantity);
                }
            }else{
                throw new Exception("Product not found");
            }
        
            if (context != null)
            {
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                };
                context.Response.Cookies.Append("InvoiceItem", JsonConvert.SerializeObject(invoiceItems), options);
            }
        }

        public Task UpdateInvoiceItemsToCookies(int productId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}