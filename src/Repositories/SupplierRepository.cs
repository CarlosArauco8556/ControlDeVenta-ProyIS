using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Helpers;
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
                throw new Exception("Supplier name already exists");
            }

            var existingSupplier1 = await _context.Suppliers.FirstOrDefaultAsync(s => s.Email == supplierDto.Email);

            if(existingSupplier1 != null)
            {
                throw new Exception("Supplier email already exists");
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

        public async Task<NewSupplierDto> DeleteSupplier(string nameSupplier)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Name == nameSupplier);
            if(existingSupplier == null)
            {
                throw new Exception("Supplier not found");
            }

            var supplierHasSupplies = existingSupplier.Supplies.Any();
            if(supplierHasSupplies)
            {
                throw new Exception("You can't delete a supplier that has supplies.");
            }

            _context.Suppliers.Remove(existingSupplier);
            await _context.SaveChangesAsync();
            return existingSupplier.MapToNewSupplierDto();
        }

        public async Task<IEnumerable<NewSupplierDto>> GetSuppliers(QueryObjectSupplier query)
        {
            var suppliers = _context.Suppliers.Include(s => s.Products).AsQueryable();
            if(!string.IsNullOrEmpty(query.textFilter))
            {
                suppliers = suppliers.Where(s => s.Name.Contains(query.textFilter) ||
                                                s.Rut.Contains(query.textFilter) ||
                                                s.PhoneNumber.Contains(query.textFilter) ||
                                                s.Email.Contains(query.textFilter) ||
                                                s.Products.Any(p => p.Name.Contains(query.textFilter)));
            }

            if(!string.IsNullOrWhiteSpace(query.orderBy))
            {
                suppliers = query.orderBy.ToLower() switch
                {
                    "name" => query.IsDescending ? suppliers.OrderByDescending(x => x.Name.ToLower()) : suppliers.OrderBy(x => x.Name.ToLower()),
                    "email" => query.IsDescending ? suppliers.OrderByDescending(x => x.Email.ToLower()) : suppliers.OrderBy(x => x.Email.ToLower()),
                    "rut" => query.IsDescending ? suppliers.OrderByDescending(x => x.Rut) : suppliers.OrderBy(x => x.Rut),
                    "phonenumber" => query.IsDescending ? suppliers.OrderByDescending(x => x.PhoneNumber) : suppliers.OrderBy(x => x.PhoneNumber),
                    _ => suppliers
                };
            }

            if(suppliers.Count() == 0)
            {
                throw new Exception("No suppliers found");
            }

            var skipNumber = (query.pageNumber - 1) * query.pageSize;

            return await suppliers.Select(s => s.MapToNewSupplierDto())
                .Skip(skipNumber)
                .Take(query.pageSize)
                .ToListAsync();
        }

        public async Task<NewSupplierDto> UpdateSupplier(string supplierName, NewSupplierDto supplierDto)
        {
            var existingSupplier = await _context.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.Name == supplierName);

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
                    throw new Exception("Supplier name already exists");
                }
            }

            bool emailChanged = existingSupplier.Email != supplierDto.Email;
            if(emailChanged)
            {
                var supplierExistsE = await _context.Suppliers.FirstOrDefaultAsync(s => s.Email == supplierDto.Email);
                if(supplierExistsE != null)
                {
                    throw new Exception("Supplier email already exists");
                }
            }

            existingSupplier.Name = supplierDto.Name;
            existingSupplier.Rut = supplierDto.Rut;
            existingSupplier.PhoneNumber = supplierDto.PhoneNumber;
            existingSupplier.Email = supplierDto.Email;

            var existingProductIds = existingSupplier.Products.Select(p => p.Id).ToHashSet();
            var productsToAssociate = await _context.Products
                .Where(p => supplierDto.ProductNames.Contains(p.Name))
                .ToListAsync();
            var newProductIds = productsToAssociate.Select(p => p.Id).ToHashSet();

            var productsToAdd = productsToAssociate.Where(p => !existingProductIds.Contains(p.Id)).ToList();
            foreach (var product in productsToAdd)
            {
                existingSupplier.Products.Add(product);
            }

            var productsToRemove = existingSupplier.Products.Where(p => !newProductIds.Contains(p.Id)).ToList();
            foreach (var product in productsToRemove)
            {
                existingSupplier.Products.Remove(product);
            }

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