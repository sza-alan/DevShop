using DevShop.Application.DTOs;

namespace DevShop.Application.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<string?> LoginAsync(LoginUserDto loginUserDto);
        Task<bool> RegisterAdminAsync(RegisterUserDto registerUserDto);
    }
}