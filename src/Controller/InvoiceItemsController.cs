using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace ControlDeVenta_Proy.src.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, Worker")]
    public class InvoiceItemsController : ControllerBase
    {
        private readonly IInvoiceItem _invoiceItemService;

        public InvoiceItemsController(IInvoiceItem invoiceItemService, IProductRepository productRepository)
        {
            _invoiceItemService = invoiceItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoiceItems()
        {
            var invoiceItems = await _invoiceItemService.GetInvoiceItemsFromCookies();
            return Ok(invoiceItems);
        }

        [HttpPost("{productId:int}")]
        public async Task<IActionResult> AddInvoiceItem(int productId)
        {
            await _invoiceItemService.SaveInvoiceItemsToCookies(productId);
            return Ok();
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> RemoveInvoiceItem(int productId)
        {
            await _invoiceItemService.RemoveInvoiceItem(productId);
            return Ok();
        }

        [HttpDelete]
        public IActionResult ClearInvoiceItems()
        {
            _invoiceItemService.ClearInvoiceItemsInCookie();
            return Ok();
        }
    }
}