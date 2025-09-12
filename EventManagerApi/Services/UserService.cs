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
        private readonly IRoleRepository _roleRepo;
        private readonly JwtHelper _jwt;

        public UserService(IUserRepository userRepo, IRoleRepository roleRepo, JwtHelper jwt)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _jwt = jwt;
        }

        public async Task<string?> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepo.GetByUsernameAsync(dto.Username);
            if (existing != null) return null;

            // Отримаємо роль "User" з БД (seed-ована)
            var role = await _roleRepo.GetByNameAsync("User");
            if (role == null) throw new InvalidOperationException("Default role 'User' not found in DB.");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = role.Id,
                Role = role // заповнимо навігацію - буде використано для JWT
            };

            await _userRepo.AddAsync(user);

            return _jwt.GenerateToken(user);
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByUsernameAsync(dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            // user.Role вже має бути загружена (GetByUsernameAsync робить Include)
            return _jwt.GenerateToken(user);
        }
    }
}
