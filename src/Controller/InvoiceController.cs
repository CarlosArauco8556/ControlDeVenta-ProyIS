using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlDeVenta_Proy.src.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, Worker")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvioce _invoiceRepository;

        public InvoiceController(IInvioce invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            try
            {
                var invoices = await _invoiceRepository.GetInvoices();
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateInvoice([FromBody] ClientDTO client)
        {
            try
            {
                var invoice = await _invoiceRepository.GenerateInvoice(client);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}