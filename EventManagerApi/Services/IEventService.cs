using EventManagerApi.Models.DTO;
using EventManagerApi.Models;

namespace EventManagerApi.Services
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<Event> CreateAsync(EventDto dto, int userId);
        Task UpdateAsync(int id, EventDto dto, int userId);
        Task DeleteAsync(int id, int userId);
    }
}
