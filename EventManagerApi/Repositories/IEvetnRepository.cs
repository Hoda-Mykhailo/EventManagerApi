using EventManagerApi.Models;

namespace EventManagerApi.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<Event> AddAsync(Event ev);
        Task UpdateAsync(Event ev);
        Task DeleteAsync(Event ev);
    }
}
