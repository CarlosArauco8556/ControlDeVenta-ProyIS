
using ControlDeVenta_Proy.src.Controller;
using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.DTOs;
using ControlDeVenta_Proy.src.Helpers;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Mappers;
using ControlDeVenta_Proy.src.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlDeVenta_Proy.src.Repositories
{
    public class SupplyRepository : ISupplyRepository
    {
        public readonly DataContext _context;
        public SupplyRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<GetSupplyDto> AddSupply(NewSupplyDto supplyDto)
        {
            var existingSupplier = await _context.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == supplyDto.SupplierId);
            if (existingSupplier == null)
            {
                throw new Exception("Supplier not found");
            }

            var existingProduct = await _context.Products.FindAsync(supplyDto.ProductId);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            var productInSupplierProducts = existingSupplier.Products.Find(p => p.Id == existingProduct.Id);
            if(productInSupplierProducts == null)
            {
                throw new Exception("Product not found in supplier products");
            }

            if(supplyDto.DeliveryDate < supplyDto.OrderDate)
            {
                throw new Exception("Delivery date must be greater than order date");
            }

            if (supplyDto.OrderDate > DateTime.Now)
            {
                throw new Exception("Order date must be less than current date");
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

            existingProduct.Stock += supply.Quantity;

            await _context.SaveChangesAsync();

            var newSupply = await _context.Supplies.FirstOrDefaultAsync(s => s.ProductId == supply.ProductId && s.SupplierId == supply.SupplierId);

            if(newSupply == null)
            {
                throw new Exception("Supply not found");
            }

            return newSupply.MapToGetSupplyDto();
        }

        public async Task<GetSupplyDto> DeleteSupply(int supplyId)
        {
            var supply = await _context.Supplies.Include(s => s.Product).Include(s => s.Supplier).FirstOrDefaultAsync(s => s.Id == supplyId);
            if(supply == null)
            {
                throw new Exception("Supply not found");
            }

            var existingProduct = await _context.Products.FindAsync(supply.ProductId);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            existingProduct.Stock -= supply.Quantity;
            
            _context.Supplies.Remove(supply);
            await _context.SaveChangesAsync();

            return supply.MapToGetSupplyDto();

        }

        public async Task<IEnumerable<GetSupplyDto>> GetSupplies(QueryObjectSupplier query)
        {
            var supplies = _context.Supplies.Include(s => s.Product).Include(s => s.Supplier).AsQueryable(); 

            if(!string.IsNullOrEmpty(query.textFilter))
            {
                supplies = supplies.Where(s => s.ProductId.ToString().Contains(query.textFilter) ||
                                               s.SupplierId.ToString().Contains(query.textFilter) ||
                                               s.OrderDate.ToString().Contains(query.textFilter) ||
                                               s.DeliveryDate.ToString().Contains(query.textFilter) ||
                                               s.Quantity.ToString().Contains(query.textFilter) ||
                                               s.TotalPrice.ToString().Contains(query.textFilter) ||
                                               s.Product.Name.ToString().Contains(query.textFilter) ||
                                               s.Supplier.Name.ToString().Contains(query.textFilter));
            }

            if(!string.IsNullOrEmpty(query.orderBy))
            {
                supplies = query.orderBy.ToLower() switch
                {
                    "productid" => query.IsDescending ? supplies.OrderByDescending(s => s.ProductId) : supplies.OrderBy(s => s.ProductId),
                    "supplierid" => query.IsDescending ? supplies.OrderByDescending(s => s.SupplierId) : supplies.OrderBy(s => s.SupplierId),
                    "orderdate" => query.IsDescending ? supplies.OrderByDescending(s => s.OrderDate) : supplies.OrderBy(s => s.OrderDate),
                    "deliverydate" => query.IsDescending ? supplies.OrderByDescending(s => s.DeliveryDate) : supplies.OrderBy(s => s.DeliveryDate),
                    "quantity" => query.IsDescending ? supplies.OrderByDescending(s => s.Quantity) : supplies.OrderBy(s => s.Quantity),
                    "totalprice" => query.IsDescending ? supplies.OrderByDescending(s => s.TotalPrice) : supplies.OrderBy(s => s.TotalPrice),
                    "productname" => query.IsDescending ? supplies.OrderByDescending(s => s.Product.Name) : supplies.OrderBy(s => s.Product.Name),
                    "suppliername" => query.IsDescending ? supplies.OrderByDescending(s => s.Supplier.Name) : supplies.OrderBy(s => s.Supplier.Name),
                    _ => supplies
                };
            }

            if(supplies.Count() == 0)
            {
                throw new Exception("Supplies not found.");
            }

            var skipNumber = (query.pageNumber - 1) * query.pageSize;

            return await supplies.Skip(skipNumber).Take(query.pageSize).Select(s => s.MapToGetSupplyDto()).ToListAsync();
        }

        public async Task<GetSupplyDto> UpdateSupply(int supplyId, NewSupplyDto supplyDto)
        {
            var existingSupply = await _context.Supplies.FindAsync(supplyId);
            if(existingSupply == null)
            {
                throw new Exception("Supply not found");
            }

            var existingSupplier = await _context.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == existingSupply.SupplierId);
            if (existingSupplier == null)
            {
                throw new Exception("Supplier not found");
            }

            var existingProduct = await _context.Products.FindAsync(existingSupply.ProductId);
            if (existingProduct == null)
            {
                throw new Exception("Product not found");
            }

            bool supplierIdChanged = existingSupply.SupplierId != supplyDto.SupplierId;
            if(supplierIdChanged)
            {
                var existingSupplierToUpdate = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == supplyDto.SupplierId);
                if (existingSupplierToUpdate == null)
                {
                    throw new Exception("The supplier to be updated was not found.");
                }

                existingSupply.Supplier = existingSupplierToUpdate;
            }

            bool productIdChanged = existingSupply.ProductId != supplyDto.ProductId;
            if(productIdChanged)
            {
                var existingProductToUpdate = await _context.Products.FindAsync(supplyDto.ProductId);
                if (existingProductToUpdate == null)
                {
                    throw new Exception("The product to be updated was not found.");
                }

                existingSupply.Product = existingProductToUpdate;
            }

            if(supplierIdChanged || productIdChanged)
            {
                var existingSupplierToUpdate = await _context.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == supplyDto.SupplierId);
                if (existingSupplierToUpdate == null)
                {
                    throw new Exception("The supplier to be updated was not found.");
                }

                var existingProductToUpdate = await _context.Products.FindAsync(supplyDto.ProductId);
                if (existingProductToUpdate == null)
                {
                    throw new Exception("The product to be updated was not found.");
                }

                var productInSupplierProducts = existingSupplierToUpdate.Products.Find(p => p.Id == existingProductToUpdate.Id);
                if(productInSupplierProducts == null)
                {
                    throw new Exception("Product not found in supplier products.");
                }
            }

            if(supplierIdChanged == false || productIdChanged)
            {
                var existingProductToUpdate = await _context.Products.FindAsync(supplyDto.ProductId);
                if (existingProductToUpdate == null)
                {
                    throw new Exception("The product to be updated was not found.");
                }

                var productInSupplierProducts = existingSupplier.Products.Find(p => p.Id == existingProductToUpdate.Id);
                if(productInSupplierProducts == null)
                {
                    throw new Exception("Product not found in supplier products.");
                }
            }

            if(supplierIdChanged == false || productIdChanged)
            {
                var existingSupplierToUpdate = await _context.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == supplyDto.SupplierId);
                if (existingSupplierToUpdate == null)
                {
                    throw new Exception("The supplier to be updated was not found.");
                }

                var productInSupplierProducts = existingSupplierToUpdate.Products.Find(p => p.Id == existingProduct.Id);
                if(productInSupplierProducts == null)
                {
                    throw new Exception("Product not found in supplier products.");
                }
            }

            bool quantityChanged = existingSupply.Quantity != supplyDto.Quantity;
            if(quantityChanged && productIdChanged == false)
            {
                existingProduct.Stock = existingProduct.Stock - existingSupply.Quantity + supplyDto.Quantity;
            }
            if(quantityChanged && productIdChanged)
            {
                existingProduct.Stock = existingProduct.Stock - existingSupply.Quantity;

                var existingProductChanged = await _context.Products.FindAsync(supplyDto.ProductId);
                if (existingProductChanged == null)
                {
                    throw new Exception("The product to be updated was not found");
                }
                existingProductChanged.Stock += supplyDto.Quantity;
            }
            if(quantityChanged == false && productIdChanged)
            {
                existingProduct.Stock = existingProduct.Stock - existingSupply.Quantity;

                var existingProductChanged = await _context.Products.FindAsync(supplyDto.ProductId);
                if (existingProductChanged == null)
                {
                    throw new Exception("The product to be updated was not found");
                }
                existingProductChanged.Stock += existingSupply.Quantity;
            }

            if(supplyDto.DeliveryDate < supplyDto.OrderDate)
            {
                throw new Exception("Delivery date must be greater than order date");
            }

            if (supplyDto.OrderDate > DateTime.Now)
            {
                throw new Exception("Order date must be less than current date");
            }

            if(supplyDto.Quantity <= 0)
            {
                throw new Exception("Quantity must be greater than 0");
            }
            
            existingSupply.SupplierId = supplyDto.SupplierId;
            existingSupply.ProductId = supplyDto.ProductId;
            existingSupply.DeliveryDate = supplyDto.DeliveryDate;
            existingSupply.OrderDate = supplyDto.OrderDate;
            existingSupply.TotalPrice = supplyDto.Quantity * existingSupply.Product.Price;
            existingSupply.Quantity = supplyDto.Quantity;  

            await _context.SaveChangesAsync();

            return existingSupply.MapToGetSupplyDto();
        }
    }
}