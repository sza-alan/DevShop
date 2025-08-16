using DevShop.Application.DTOs;
using DevShop.Application.Implementation;
using DevShop.Application.Interfaces;
using DevShop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace DevShop.UnitTests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHashingService> _passwordHashingServiceMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHashingServiceMock = new Mock<IPasswordHashingService>();
            _configurationMock = new Mock<IConfiguration>();

            _authService = new AuthService(
                _userRepositoryMock.Object,
                _passwordHashingServiceMock.Object,
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFalse_WhenUserAlreadyExists()
        {
            var registerDto = new RegisterUserDto { Email = "test@test.com", Password = "password" };
            var existingUser = new User(registerDto.Email, "hashedpassword", "Customer");

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(registerDto.Email)).ReturnsAsync(existingUser);

            var result = await _authService.RegisterUserAsync(registerDto);

            Assert.False(result);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnTrue_WhenUserDoesNotExist()
        {
            var registerDto = new RegisterUserDto { Email = "newuser@test.com", Password = "password" };

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(registerDto.Email)).ReturnsAsync((User?)null);

            _passwordHashingServiceMock.Setup(service => service.Hash(registerDto.Password)).Returns("hashedpassword");

            var result = await _authService.RegisterUserAsync(registerDto);

            Assert.True(result);
            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenUserNotFound()
        {
            var loginDto = new LoginUserDto { Email = "nonexistent@test.com", Password = "password" };
            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(loginDto.Email))
                               .ReturnsAsync((User?)null);

            var token = await _authService.LoginAsync(loginDto);

            Assert.Null(token);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            var loginDto = new LoginUserDto { Email = "user@test.com", Password = "wrongpassword" };
            var existingUser = new User(loginDto.Email, "correct_hash", "Customer");

            _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(loginDto.Email))
                               .ReturnsAsync(existingUser);

            _passwordHashingServiceMock.Setup(service => service.Verify(loginDto.Password, existingUser.PasswordHash))
                                       .Returns(false);

            var token = await _authService.LoginAsync(loginDto);

            Assert.Null(token);
        }
    }
}
