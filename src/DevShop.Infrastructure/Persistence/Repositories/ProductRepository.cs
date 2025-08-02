using DevShop.Application.Interfaces;
using DevShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DevShopDbContext _context;

        public ProductRepository(DevShopDbContext context) => _context = context;

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
    }
}
