using ControlDeVenta_Proy.src.Helpers;
using ControlDeVenta_Proy.src.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlDeVenta_Proy.src.Controller
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(Roles = "Admin, User")]
    public class SupplierController : ControllerBase
    {
        public readonly ISupplierRepository  _supplierRepository;
        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers([FromQuery] QueryObjectSupplier query)
        {
            try
            {
                var suppliers = await _supplierRepository.GetSuppliers(query);
                return Ok(suppliers);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}