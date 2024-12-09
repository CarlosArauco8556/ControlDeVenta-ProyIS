using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlDeVenta_Proy.src.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProductManagementController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductManagementController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] NewPorductDto product)
        {
            try
            {
                var newProduct = await _productRepository.AddProduct(product.MapToCreateProduct());
                return Ok(newProduct);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] NewPorductDto product)
        {
            try
            {
                var updatedProduct = await _productRepository.UpdateProduct(id, product);
                return Ok(updatedProduct);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var deletedProduct = await _productRepository.DeleteProduct(id);
                return Ok(deletedProduct);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}