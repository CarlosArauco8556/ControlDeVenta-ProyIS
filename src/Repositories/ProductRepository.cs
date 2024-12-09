using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<NewPorductDto> AddProduct(Product product)
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

        public Task<NewPorductDto> DeleteProduct(int id)
        {
            throw new NotImplementedException();
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

        public Task<NewPorductDto> UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}