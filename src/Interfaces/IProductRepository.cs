using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Helpers;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<NewPorductDto>> GetProducts(QueryObject query);
        Task<Product> GetProductById(int id);
        Task<NewPorductDto> AddProduct(Product product);
        Task<NewPorductDto> UpdateProduct(int id, NewPorductDto product);
        Task<NewPorductDto> DeleteProduct(int id);
    }
}