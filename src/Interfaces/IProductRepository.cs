using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Helpers;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts(QueryObject query);
        Task<Product> AddProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<Product> DeleteProduct(int id);
    }
}