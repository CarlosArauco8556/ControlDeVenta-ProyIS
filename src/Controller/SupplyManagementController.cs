using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlDeVenta_Proy.src.Controller
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(Roles = "Admin")]
    public class SupplyManagementController : ControllerBase
    {
        public readonly ISupplyRepository _supplyrepository;
        public SupplyManagementController(ISupplyRepository supplyRepository)
        {
            _supplyrepository = supplyRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddSupply([FromBody] NewSupplyDto supplyDto)
        {
            try
            {
                var newSupply = await _supplyrepository.AddSupply(supplyDto);
                return Ok(new { Message = "Supply added.", newSupply});
            }catch (Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{supplyId}")]
        public async Task<IActionResult> UpdateSupply(int supplyId, [FromBody] NewSupplyDto supplyDto)
        {
            try
            {
                var updatedSuply = await _supplyrepository.UpdateSupply(supplyId, supplyDto);
                return Ok(new { Message = "Supply updated.", updatedSuply});
            } catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{supplyId}")]
        public async Task<IActionResult> DeleteSupply(int supplyId)
        {
            try
            {
                var deletedSupply = await _supplyrepository.DeleteSupply(supplyId);
                return Ok(new { Message = "Supply deleted.", deletedSupply});
            } catch (Exception e){
                return BadRequest(e.Message);
            }
        }
    } 
}