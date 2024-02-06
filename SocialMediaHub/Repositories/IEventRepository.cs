using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public interface IEventRepository
    {
        Task<IQueryable<Event>> GetAllEvents();
        Task<Event> GetEvent(int eventId);
        Task AddEvent(Event _event);
        Task UpdateEvent(Event _event);
        Task RemoveEvent(int eventId);
        Task<byte[]> GetEventsCsvBytes();
        Task<IEnumerable<Event>> SearchEvents(string searchTerm);
        Task<IEnumerable<Event>> SearchPartialEvents(string searchTerm);
        Task AddUserToEvent(int eventId, int userId);
    }
}
