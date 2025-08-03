using DevShop.Application.Interfaces;
using DevShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Infrastructure.Persistence
{
    public class DevShopDbContext : DbContext, IUnitOfWork
    {
        public DevShopDbContext(DbContextOptions<DevShopDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
