using EventManagerApi.Data;
using EventManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagerApi.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context) => _context = context;

        public async Task<Role?> GetByNameAsync(string name) =>
            await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);

        public async Task<Role?> GetByIdAsync(int id) =>
            await _context.Roles.FindAsync(id);

        public async Task<IEnumerable<Role>> GetAllAsync() =>
            await _context.Roles.ToListAsync();
    }
}
