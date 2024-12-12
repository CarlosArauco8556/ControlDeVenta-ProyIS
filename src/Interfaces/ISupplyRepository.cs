
using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Helpers;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface ISupplyRepository
    {
        Task<IEnumerable<GetSupplyDto>> GetSupplies(QueryObjectSupplier query);
        Task<GetSupplyDto> AddSupply(NewSupplyDto supplyDto);
        Task<GetSupplyDto> UpdateSupply(int productId, int supplyId, NewSupplyDto supplyDto);
        Task<GetSupplyDto> DeleteSupply(int productId, int supplyId);
    }
}