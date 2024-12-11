
using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Helpers;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface ISupplyRepsitory
    {
        Task<IEnumerable<NewSupplyDto>> GetSupplies(QueryObjectSupplier query);
        Task<NewSupplyDto> AddSupply(NewSupplyDto supplyDto);
        Task<NewSupplierDto> UpdateSupply(int productId, int supplierId, NewSupplierDto supplierDto);
        Task<NewSupplierDto> DeleteSupply(int productId, int supplierId);
    }
}