using DevShop.Domain.Entities;

namespace DevShop.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<bool> HasAnyUserAsync();
    }
}
