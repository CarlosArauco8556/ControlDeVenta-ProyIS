using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Mappers;
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
        
        public async Task<NewSupplierDto> AddSupplier(NewSupplierDto supplierDto)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Email == supplierDto.Email);

            if(existingSupplier != null)
            {
                throw new Exception("Supplier already exists");
            }

            foreach (string productName in supplierDto.ProductNames)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == productName);

                if(product == null)
                {
                    throw new Exception("Product not found");
                }
            }

            var supplier = new Supplier
            {
                Name = supplierDto.Name,
                Rut = supplierDto.Rut,
                PhoneNumber = supplierDto.PhoneNumber,
                Email = supplierDto.Email,
                Products = await _context.Products.Where(p => supplierDto.ProductNames.Contains(p.Name)).ToListAsync(),
            };

            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();

            var newSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Email == supplier.Email);

            if(newSupplier == null)
            {
                throw new Exception("Supplier not found");
            }

            return newSupplier.MapToNewSupplierDto();
        }

        public Task<NewSupplierDto> DeleteSupplier(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NewSupplierDto>> GetSuppliers()
        {
            throw new NotImplementedException();
        }

        public Task<NewSupplierDto> UpdateSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }
    }
}