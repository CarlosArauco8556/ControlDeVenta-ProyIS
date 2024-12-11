
using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Mappers
{
    public static class SupplyMapper
    {
        public static GetSupplyDto MapToGetSupplyDto(this Supply supply)
        {
            return new GetSupplyDto
            {
                OrderDate = supply.OrderDate,
                DeliveryDate = supply.DeliveryDate,
                Quantity = supply.Quantity,
                TotalPrice = supply.TotalPrice,
                ProductId = supply.ProductId,
                SupplierId = supply.SupplierId,
            };
        }

        public static Supply MapToSupply(this NewSupplyDto newSupplyDto)
        {
            return new Supply
            {
                OrderDate = newSupplyDto.OrderDate,
                DeliveryDate = newSupplyDto.DeliveryDate,
                Quantity = newSupplyDto.Quantity,
                ProductId = newSupplyDto.ProductId,
                SupplierId = newSupplyDto.SupplierId,
            };
        }
    }
}