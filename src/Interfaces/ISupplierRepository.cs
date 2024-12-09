using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Helpers;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<NewSupplierDto>> GetSuppliers(QueryObjectSupplier query);
        Task<NewSupplierDto> AddSupplier(NewSupplierDto supplierDto);
        Task<NewSupplierDto> UpdateSupplier(string supplierName, NewSupplierDto supplierDto);
        Task<NewSupplierDto> DeleteSupplier(int id);
    }
}