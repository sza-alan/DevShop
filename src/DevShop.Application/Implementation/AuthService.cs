using DevShop.Application.DTOs;
using DevShop.Application.Interfaces;
using DevShop.Application.Services;
using DevShop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevShop.Application.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashingService _passwordHashingService;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUserRepository userRepository, IPasswordHashingService passwordHashingService, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHashingService = passwordHashingService;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> LoginAsync(LoginUserDto loginUserDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginUserDto.Email);
            if (user == null)
                return null;

            var isPasswordValid = _passwordHashingService.Verify(loginUserDto.Password, user.PasswordHash);
            if (!isPasswordValid)
                return null;

            var token = GenerateJwtToken(user);
            return token;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var existingUser = _userRepository.GetByEmailAsync(registerUserDto.Email);
            if (existingUser == null) 
                return false;

            var passwordHash = _passwordHashingService.Hash(registerUserDto.Password);

            var user = new User(registerUserDto.Email, passwordHash, "Customer");

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var secret = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secret))
                throw new InvalidOperationException("JWT Secret is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
