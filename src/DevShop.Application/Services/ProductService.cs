using DevShop.Application.DTOs;
using DevShop.Application.Interfaces;
using DevShop.Domain.Entities;

namespace DevShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository) => _productRepository = productRepository;

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(CreateProductDto productDto)
        {
            var product = new Product(
                productDto.Name,
                productDto.Description,
                productDto.Price,
                productDto.Stock
            );

            await _productRepository.AddAsync(product);

            return product;
        }
    }
}
