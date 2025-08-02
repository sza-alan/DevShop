using DevShop.Application.DTOs;
using DevShop.Domain.Entities;

namespace DevShop.Application.Services
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(CreateProductDto productDto);
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
