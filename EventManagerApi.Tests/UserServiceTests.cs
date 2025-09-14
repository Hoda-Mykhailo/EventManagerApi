using EventManagerApi.Models;
using EventManagerApi.Models.DTO;
using EventManagerApi.Repositories;
using EventManagerApi.Services;
using EventManagerApi.Helpers;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace EventManagerApi.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IRoleRepository> _roleRepoMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            // Тестова конфігурація для JwtHelper
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"Jwt:Key", "TestKey1234567890TestKey1234567890"},   // довільний ключ, достатньо довгий
                    {"Jwt:Issuer", "PassSecretKey0011333"}
                }!)
                .Build();

            _userRepoMock = new Mock<IUserRepository>();
            _roleRepoMock = new Mock<IRoleRepository>();

            _roleRepoMock
                .Setup(r => r.GetByNameAsync("User"))
                .ReturnsAsync(new Role { Id = 1, Name = "User" });

            // Використовуємо реальний JwtHelper (метод GenerateToken не треба мокати)
            var jwtHelper = new JwtHelper(configuration);

            // ВАЖЛИВО: створюємо _userService і зберігаємо у полі
            _userService = new UserService(
                _userRepoMock.Object,
                _roleRepoMock.Object,
                jwtHelper);
        }

        [Fact]
        public async Task RegisterAsync_ShouldCreateUser_AndReturnToken()
        {
            var dto = new RegisterDto { Username = "test", Password = "1234" };

            // Юзер з таким ім'ям не існує
            _userRepoMock.Setup(r => r.GetByUsernameAsync("test"))
                         .ReturnsAsync((User?)null);

            // Після створення повертаємо юзера
            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                         .ReturnsAsync(new User { Id = 1, Username = "test" });

            var token = await _userService.RegisterAsync(dto);

            token.Should().NotBeNullOrEmpty(); // головна перевірка
            // Додатково можна перевірити, що це дійсний JWT:
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            jwt.Issuer.Should().Be("PassSecretKey0011333");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_IfUserNotFound()
        {
            var dto = new LoginDto { Username = "user", Password = "1234" };

            _userRepoMock.Setup(r => r.GetByUsernameAsync("user"))
                         .ReturnsAsync((User?)null);

            var result = await _userService.LoginAsync(dto);

            result.Should().BeNull();
        }
    }
}
