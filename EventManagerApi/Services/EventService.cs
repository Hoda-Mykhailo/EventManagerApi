using EventManagerApi.Models;
using EventManagerApi.Models.DTO;
using EventManagerApi.Repositories;

namespace EventManagerApi.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _repo;

        public EventService(IEventRepository repo) => _repo = repo;

        public async Task<IEnumerable<Event>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<Event?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<Event> CreateAsync(EventDto dto, int userId)
        {
            var ev = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                UserId = userId
            };
            return await _repo.AddAsync(ev);
        }

        public async Task UpdateAsync(int id, EventDto dto, int userId)
        {
            var ev = await _repo.GetByIdAsync(id);
            if (ev == null || ev.UserId != userId) throw new UnauthorizedAccessException();

            ev.Title = dto.Title;
            ev.Description = dto.Description;
            ev.Date = dto.Date;
            await _repo.UpdateAsync(ev);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var ev = await _repo.GetByIdAsync(id);
            if (ev == null || ev.UserId != userId) throw new UnauthorizedAccessException();

            await _repo.DeleteAsync(ev);
        }
    }
}
