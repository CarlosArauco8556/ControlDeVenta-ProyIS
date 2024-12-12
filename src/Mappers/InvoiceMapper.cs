using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Mappers
{
    public static class InvoiceMapper
    {
        public static InvoiceDto MapInvoiceToDTO(Invoice invoice, List<SaleItemDto> saleItemDtos)
        {
            return new InvoiceDto
            {
                Factura = invoice.Id,
                Descripcion = invoice.Description,
                Fecha = invoice.CreationDate,
                PriceWithoutVAT = invoice.PriceWithoutVAT,
                TotalVAT = invoice.TotalVAT,
                FinalPrice = invoice.FinalPrice,
                SaleItems = saleItemDtos
            };
        }

        public static List<SaleItemDto> ToSaleItemDto(List<SaleItem> saleItems)
        {
            var saleItemDtos = new List<SaleItemDto>();

            foreach (var saleItem in saleItems)
            {
                saleItemDtos.Add(new SaleItemDto
                {
                    ProductId = saleItem.ProductId,
                    Name = saleItem.Product.Name,
                    Quantity = saleItem.Quantity,
                    Price = saleItem.UnitPrice,
                    DiscountPercentage = saleItem.Product.DiscountPercentage,
                    Total = saleItem.Quantity * saleItem.UnitPrice
                });
            }

            return saleItemDtos;
        }
    }
}