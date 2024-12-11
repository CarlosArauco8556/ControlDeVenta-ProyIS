
using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Helpers;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Mappers;
using ControlDeVenta_Proy.src.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlDeVenta_Proy.src.Repositories
{
    public class SupplyRepository : ISupplyRepsitory
    {
        public readonly DataContext _context;
        public SupplyRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<GetSupplyDto> AddSupply(NewSupplyDto supplyDto)
        {
            var existingSupplier = await _context.Suppliers.FindAsync(supplyDto.SupplierId);
            if (existingSupplier == null)
            {
                throw new Exception("Supplier not found");
            }

            var existingProduct = await _context.Products.FindAsync(supplyDto.ProductId);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            if(supplyDto.DeliveryDate < supplyDto.OrderDate)
            {
                throw new Exception("Delivery date must be greater than order date");
            }

            if (supplyDto.OrderDate > DateTime.Now)
            {
                throw new Exception("Order date must be less than current date");
            }

            if (supplyDto.DeliveryDate != DateTime.Now)
            {
                throw new Exception("Delivery date must be equal to current date");
            }

            if(supplyDto.Quantity <= 0)
            {
                throw new Exception("Quantity must be greater than 0");
            }

            var supply = supplyDto.MapToSupply();
            supply.Product = existingProduct;
            supply.Supplier= existingSupplier;
            supply.TotalPrice = supply.Quantity * existingProduct.Price;

            await _context.Supplies.AddAsync(supply);
            await _context.SaveChangesAsync();

            var newSupply = await _context.Supplies.FirstOrDefaultAsync(s => s.ProductId == supply.ProductId && s.SupplierId == supply.SupplierId);

            if(newSupply == null)
            {
                throw new Exception("Supply not found");
            }

            return newSupply.MapToGetSupplyDto();
        }

        public Task<GetSupplyDto> DeleteSupply(int productId, int supplierId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetSupplyDto>> GetSupplies(QueryObjectSupplier query)
        {
            throw new NotImplementedException();
        }

        public Task<GetSupplyDto> UpdateSupply(int productId, int supplierId, NewSupplyDto supplyDto)
        {
            throw new NotImplementedException();
        }
    }
}