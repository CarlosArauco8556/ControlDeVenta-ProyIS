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
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Name == supplierDto.Name);

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

        public async Task<NewSupplierDto> DeleteSupplier(int id)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
            if(existingSupplier == null)
            {
                throw new Exception("Supplier not found");
            }

            _context.Suppliers.Remove(existingSupplier);
            await _context.SaveChangesAsync();
            return existingSupplier.MapToNewSupplierDto();
        }

        public Task<IEnumerable<NewSupplierDto>> GetSuppliers()
        {
            throw new NotImplementedException();
        }

        public async Task<NewSupplierDto> UpdateSupplier(string supplierName, NewSupplierDto supplierDto)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Name == supplierName);

            if(existingSupplier == null)
            {
                throw new Exception("Supplier not found");
            }

            foreach (string productName in supplierDto.ProductNames)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == productName);

                if(product == null)
                {
                    throw new Exception("Product not found");
                }
            }

            bool nameChanged = existingSupplier.Name != supplierDto.Name;
            if(nameChanged)
            {
                var supplierExists = await _context.Suppliers.FirstOrDefaultAsync(s => s.Name == supplierDto.Name);
                if(supplierExists != null)
                {
                    throw new Exception("Supplier already exists");
                }
            }

            existingSupplier.Name = supplierDto.Name;
            existingSupplier.Rut = supplierDto.Rut;
            existingSupplier.PhoneNumber = supplierDto.PhoneNumber;
            existingSupplier.Email = supplierDto.Email;
            existingSupplier.Products = await _context.Products.Where(p => supplierDto.ProductNames.Contains(p.Name)).ToListAsync();

            await _context.SaveChangesAsync();

            var supplierUpdated = await _context.Suppliers.FirstOrDefaultAsync(s => s.Name == supplierDto.Name);

            if(supplierUpdated == null)
            {
                throw new Exception("Failde to update supplier");
            }

            return supplierUpdated.MapToNewSupplierDto();
        }
    }
}