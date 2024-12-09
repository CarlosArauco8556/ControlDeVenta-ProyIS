using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Mappers
{
    public static class SupplierMapper
    {
        public static NewSupplierDto MapToNewSupplierDto(this Supplier supplier)
        {
            return new NewSupplierDto
            {
                Name = supplier.Name,
                Rut = supplier.Rut,
                PhoneNumber = supplier.PhoneNumber,
                Email = supplier.Email,
                ProductNames = supplier.Products.ConvertAll(p => p.Name),
            };
        }
    }
}