using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Mappers
{
    public static class ProductMapper
    {
        public static Product MapToCreateProduct(this NewPorductDto newProductDto)
        {
            return new Product
            {
                Name = newProductDto.Name,
                Price = newProductDto.Price,
                DiscountPercentage = newProductDto.DiscountPercentage,
                Stock = newProductDto.Stock,
                StockMin = newProductDto.StockMin
            };
        }

        public static NewPorductDto MapToNewProductDto(this Product product)
        {
            return new NewPorductDto
            {
                Name = product.Name,
                Price = product.Price,
                DiscountPercentage = product.DiscountPercentage,
                Stock = product.Stock,
                StockMin = product.StockMin
            };
        }
    }
}