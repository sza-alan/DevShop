using DevShop.Infrastructure.Security;
using Xunit.Abstractions;

namespace DevShop.UnitTests
{
    public class PasswordHashingServiceTests
    {
        private readonly ITestOutputHelper _output;

        public PasswordHashingServiceTests(ITestOutputHelper output) => _output = output;

        [Fact]
        public void Hash_ShouldReturn_ValidBcryptHash()
        {
            var service = new PasswordHashingService();
            var password = "password123";

            var hash = service.Hash(password);

            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
            Assert.True(BCrypt.Net.BCrypt.Verify(password, hash));
        }

        [Fact]
        public void Verify_ShouldReturnTrue_ForCorrectPassword()
        {
            var service = new PasswordHashingService();
            var password = "password123";

            var hash = service.Hash(password);

            var result = service.Verify(password, hash);

            Assert.True(result);
        }

        [Fact]
        public void Verify_ShouldReturnFalse_ForIncorrectPassword()
        {
            var service = new PasswordHashingService();
            var password = "password123";
            var incorrectPassword = "wrongpassword";
            var hash = service.Hash(password);

            _output.WriteLine($"Hash Gerado: {hash}");

            var result = service.Verify(incorrectPassword, hash);

            Assert.False(result);
        }
    }
}
