using BCrypt.Net;
using EventManagerApi.Models;
using EventManagerApi.Models.DTO;
using EventManagerApi.Repositories;
using EventManagerApi.Helpers;

namespace EventManagerApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtHelper _jwt;

        public UserService(IUserRepository userRepo, JwtHelper jwt)
        {
            _userRepo = userRepo;
            _jwt = jwt;
        }

        public async Task<string?> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepo.GetByUsernameAsync(dto.Username);
            if (existing != null) return null;

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _userRepo.AddAsync(user);
            return _jwt.GenerateToken(user);
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByUsernameAsync(dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            return _jwt.GenerateToken(user);
        }
    }
}
