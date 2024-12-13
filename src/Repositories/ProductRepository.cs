using ControlDeVenta_Proy.src.Data;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Helpers;
using ControlDeVenta_Proy.src.Interfaces;
using ControlDeVenta_Proy.src.Mappers;
using ControlDeVenta_Proy.src.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlDeVenta_Proy.src.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<NewProductDto> AddProduct(Product product)
        {

            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == product.Name);

            if (existingProduct != null)
            {
                throw new Exception("Product already exists.");
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var newProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == product.Name);

            if (newProduct == null)
            {
                throw new Exception("Failed to add new product.");
            }

            return newProduct.MapToNewProductDto();
        }

        public async Task<NewProductDto> DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }
            
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product.MapToNewProductDto();
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            return product ?? throw new Exception("Product not found.");
        }

        public async Task<IEnumerable<NewPorductDto>> GetProducts(QueryObject query)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(query.textFilter))
            {

                products = products.Where(p => p.Name.Contains(query.textFilter) ||
                                               p.Price.ToString().Contains(query.textFilter) ||
                                               p.Stock.ToString().Contains(query.textFilter));
                
                if(!products.Any())
                {
                    throw new Exception("Product not found.");
                }
            }

            if(!string.IsNullOrWhiteSpace(query.sortByPrice))
            {
                if(query.sortByPrice.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    products = query.IsDescending ? products.OrderByDescending(x => x.Price) : products.OrderBy(x => x.Price);
                }
            }

            var skipNumber = (query.pageNumber - 1) * query.pageSize;

            return await products.Select(p => p.MapToNewProductDto())
                .Skip(skipNumber)
                .Take(query.pageSize)
                .ToListAsync();
        }

        public async Task<NewProductDto> UpdateProduct(int id, NewProductDto product)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            
            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            bool nameChanged = existingProduct.Name != product.Name;

            if (nameChanged)
            {
                var productValidation = await _context.Products
                    .Where(p => p.Name == product.Name)
                    .FirstOrDefaultAsync();

                if (productValidation != null)
                {
                    throw new Exception("Product already exists.");
                }
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.StockMin = product.StockMin;
            existingProduct.DiscountPercentage = product.DiscountPercentage;
            
            await _context.SaveChangesAsync();

            var productUpdated = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (productUpdated == null)
            {
                throw new Exception("Failed to update product.");
            }

            return productUpdated.MapToNewProductDto();
        }
    }
}