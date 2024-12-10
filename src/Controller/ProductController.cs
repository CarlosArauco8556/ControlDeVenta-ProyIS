using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Helpers;
using ControlDeVenta_Proy.src.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlDeVenta_Proy.src.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, Worker")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] QueryObject query)
        {
            try
            {
                var products = await _productRepository.GetProducts(query);
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}