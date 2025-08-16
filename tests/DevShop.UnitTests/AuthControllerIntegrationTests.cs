using DevShop.Application.DTOs;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DevShop.UnitTests
{
    public class AuthControllerIntegrationTests : IClassFixture<IntegrationTestWebAppFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(IntegrationTestWebAppFactory factory) => _client = factory.CreateClient();

        [Fact]
        public async Task RegisterAndLogin_ShouldSucceed_WithValidCredentials()
        {
            var userEmail = $"testuser-{System.Guid.NewGuid()}@example.com";
            var userPassword = "Password123!";
            var registerDto = new RegisterUserDto { Email = userEmail, Password = userPassword };

            var registerResponse = await _client.PostAsJsonAsync("/auth/register", registerDto);

            Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);

            var loginDto = new LoginUserDto { Email = userEmail, Password = userPassword };

            var loginResponse = await _client.PostAsJsonAsync("/auth/login", loginDto);

            loginResponse.EnsureSuccessStatusCode();
            var responseString = await loginResponse.Content.ReadAsStringAsync();
            var jsonResponse = JsonDocument.Parse(responseString);
            var token = jsonResponse.RootElement.GetProperty("token").GetString();

            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }
    }
}
