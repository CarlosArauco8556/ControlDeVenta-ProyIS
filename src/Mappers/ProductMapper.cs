using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Mappers
{
    public static class ProductMapper
    {
        public static Product MapToCreateProduct(this NewProductDto newProductDto)
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

        public static NewProductDto MapToNewProductDto(this Product product)
        {
            return new NewProductDto
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