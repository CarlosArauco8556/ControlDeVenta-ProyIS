using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Mappers
{
    public static class SupplierMapper
    {
        public static Supplier MapToNewSupplier(this NewSupplierDto supplierDto)
        {
            return new Supplier
            {
                Name = supplierDto.Name,
                Rut = supplierDto.Rut,
                PhoneNumber = supplierDto.PhoneNumber,
                Email = supplierDto.Email,

            };
        }
        public static NewSupplierDto MapToNewSupplierDto(this Supplier supplier)
        {
            
        }
    }
}