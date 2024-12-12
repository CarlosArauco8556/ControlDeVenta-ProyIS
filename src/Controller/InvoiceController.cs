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
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            
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
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

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

        [HttpPut("{ivoiceId:int}/{productId:int}/{newProductId:int}/{quantity:int}/{isAddition:bool}")]
        public async Task<IActionResult> UpdateInvoice1([FromRoute] int ivoiceId, [FromRoute] int productId, [FromRoute] int? newProductId, [FromRoute] int? quantity, [FromRoute] bool? isAddition)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            if (ivoiceId <= 0)
            {
                return BadRequest("Invalid invoice ID.");
            }

            if (productId <= 0)
            {
                return BadRequest("Invalid product ID.");
            }

            if (newProductId <= 0)
            {
                return BadRequest("Invalid new product ID.");
            }

            if (quantity <= 0)
            {
                return BadRequest("Invalid quantity.");
            }

            try
            {
                var updatedInvoice = await _invoiceRepository.UpdateInvoiceItem(ivoiceId, productId, newProductId, quantity, isAddition);
                return Ok(updatedInvoice);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{invoiceId:int}")]
        public async Task<IActionResult> UpdateInvoice2([FromRoute] int invoiceId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            if (invoiceId <= 0)
            {
                return BadRequest("Invalid invoice ID.");
            }

            try
            {
                await _invoiceRepository.UpdateInvoiceStatus(invoiceId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{invoiceId:int}")]
        public async Task<IActionResult> DeleteInvoice([FromRoute] int invoiceId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            if (invoiceId <= 0)
            {
                return BadRequest("Invalid invoice ID.");
            }

            try
            {
                await _invoiceRepository.DeleteInvoice(invoiceId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}