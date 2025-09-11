using EventManagerApi.Data;
using EventManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagerApi.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;
        public EventRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Event>> GetAllAsync() =>
            await _context.Events.Include(e => e.User).ToListAsync();

        public async Task<Event?> GetByIdAsync(int id) =>
            await _context.Events.Include(e => e.User).FirstOrDefaultAsync(e => e.Id == id);

        public async Task<Event> AddAsync(Event ev)
        {
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return ev;
        }

        public async Task UpdateAsync(Event ev)
        {
            _context.Events.Update(ev);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Event ev)
        {
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
        }
    }
}
