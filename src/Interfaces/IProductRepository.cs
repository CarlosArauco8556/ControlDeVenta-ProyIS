using ControlDeVenta_Proy.src.Dtos;
using ControlDeVenta_Proy.src.Helpers;
using ControlDeVenta_Proy.src.Models;

namespace ControlDeVenta_Proy.src.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<NewProductDto>> GetProducts(QueryObject query);
        Task<Product> GetProductById(int id);
        Task<NewProductDto> AddProduct(Product product);
        Task<NewProductDto> UpdateProduct(int id, NewProductDto product);
        Task<NewProductDto> DeleteProduct(int id);

    }
}