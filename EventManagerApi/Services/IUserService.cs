using EventManagerApi.Models.DTO;
using EventManagerApi.Models;

namespace EventManagerApi.Services
{
    public interface IUserService
    {
        Task<string?> RegisterAsync(RegisterDto dto);
        Task<string?> LoginAsync(LoginDto dto);
    }
}
