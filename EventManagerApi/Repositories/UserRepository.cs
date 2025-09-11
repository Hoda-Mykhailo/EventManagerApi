using EventManagerApi.Data;
using EventManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagerApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        // Тепер включаємо Role навігацію
        public async Task<User?> GetByUsernameAsync(string username) =>
            await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Завантажимо роль навігаційно (щоб caller мав user.Role)
            await _context.Entry(user).Reference(u => u.Role).LoadAsync();

            return user;
        }
    }
}
