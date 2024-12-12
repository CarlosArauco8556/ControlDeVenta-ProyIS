using ControlDeVenta_Proy.src.Helpers;
using ControlDeVenta_Proy.src.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlDeVenta_Proy.src.Controller
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(Roles= "Admin, User")]
    public class SupplyController : ControllerBase
    {
        public readonly ISupplyRepository _supplyRepository;
        public SupplyController(ISupplyRepository supplyRepository)
        {
            _supplyRepository = supplyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSupplies([FromQuery] QueryObjectSupplier query)
        {
            try
            {
                var supplies = await _supplyRepository.GetSupplies(query);
                return Ok(supplies);
            } catch (Exception e){
                return BadRequest(e.Message);
            }
        }
    }
}