using EventManagerApi.Models;

namespace EventManagerApi.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User> AddAsync(User user);
    }
}
