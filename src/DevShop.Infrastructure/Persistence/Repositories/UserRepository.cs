using DevShop.Application.Interfaces;
using DevShop.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DevShopDbContext _context;

        public UserRepository(DevShopDbContext context) => _context = context;

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user) => await _context.Users.AddAsync(user);
    }
}
