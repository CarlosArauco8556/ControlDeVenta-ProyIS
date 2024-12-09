using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<NewSupplierDto>> GetSuppliers();
        Task<NewSupplierDto> AddSupplier(NewSupplierDto supplierDto);
        Task<NewSupplierDto> UpdateSupplier(Supplier supplier);
        Task<NewSupplierDto> DeleteSupplier(int id);
    }
}