using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlDeVenta_Proy.src.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        public readonly DataContext _context;

        public SupplierRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<NewSupplierDto> AddSupplier(Supplier supplier)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Email == supplier.Email);

            if(existingSupplier != null)
            {
                throw new Exception("Supplier already exists");
            }

            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();

            var newSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Email == supplier.Email);

            if(newSupplier == null)
            {
                throw new Exception("Supplier not found");
            }

            return newSupplier.MapToNewSupplierDto();
        }

        public async Task<NewSupplierDto> DeleteSupplier(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NewSupplierDto>> GetSuppliers()
        {
            throw new NotImplementedException();
        }

        public async Task<NewSupplierDto> UpdateSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }
    }
}