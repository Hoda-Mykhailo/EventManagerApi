using EventManagerApi.Models;
using EventManagerApi.Models.DTO;
using EventManagerApi.Repositories;
using EventManagerApi.Services;
using EventManagerApi.Helpers;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Castle.Core.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace EventManagerApi.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<JwtHelper> _jwtHelperMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                { "Jwt:Key", "TEST_KEY_1234567890" }
                }!)
                .Build();

            _jwtHelperMock = new Mock<JwtHelper>(config);
            _jwtHelperMock.Setup(j => j.GenerateToken(It.IsAny<User>()))
            .Returns("FAKE_JWT_TOKEN");

            var roleRepoMock = new Mock<IRoleRepository>();

            var jwtHelper = new JwtHelper(config);
            var service = new UserService(_userRepoMock.Object, roleRepoMock.Object, jwtHelper);


        }
        // Have bugs with this test
        [Fact]
        public async Task RegisterAsync_ShouldCreateUser_AndReturnToken()
        {
            var dto = new RegisterDto { Username = "test", Password = "1234" };
            _userRepoMock.Setup(r => r.GetByUsernameAsync("test"))
                         .ReturnsAsync((User?)null);

            _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                         .ReturnsAsync(new User { Id = 1, Username = "test" });

            var token = await _userService.RegisterAsync(dto);

            token.Should().NotBeNull();
            token.Should().Be("FAKE_JWT_TOKEN");
        }

        //this test have bugs too.
        [Fact]
        public async Task LoginAsync_ShouldReturnNull_IfUserNotFound()
        {
            var dto = new LoginDto { Username = "user1", Password = "1234" };
            _userRepoMock.Setup(r => r.GetByUsernameAsync("user1"))
                         .ReturnsAsync((User?)null);

            var result = await _userService.LoginAsync(dto);

            result.Should().BeNull();
        }
    }
}
