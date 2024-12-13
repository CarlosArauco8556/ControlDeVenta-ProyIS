
using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlDeVenta_Proy.src.Controller
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(Roles = "Admin")]
    public class SuppliersManagementController : ControllerBase
    {
        public readonly ISupplierRepository _supplierRepository;

        public SuppliersManagementController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromBody] NewSupplierDto supplierDto)
        {
            try
            {
                var newSupplier = await _supplierRepository.AddSupplier(supplierDto);
                return Ok(new{Message="Supplier added.", newSupplier});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{nameSupplier}")]
        public async Task<IActionResult> UpdateSupplier(string nameSupplier, [FromBody] NewSupplierDto supplierDto)
        {
            try
            {
                var updatedSupplier = await _supplierRepository.UpdateSupplier(nameSupplier, supplierDto);
                return Ok(new{Message="Supplier updated.", updatedSupplier});
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{supplierName}")]
        public async Task<IActionResult> deleteSupplier(string supplierName)
        {
            try
            {
                var deletedSupplier = await _supplierRepository.DeleteSupplier(supplierName);
                return Ok(new{Message="Supplier deleted.", deletedSupplier});
            }catch (Exception e){
                return BadRequest(e.Message);
            }
        }    
        
    }
}