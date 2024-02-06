using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using SocialMediaHub.Database;
using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Event>> GetAllEvents()
            => await Task.FromResult(_context.Events.OrderBy(e => e.Id));

        public async Task<Event> GetEvent(int eventId)
            => await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);

        public async Task AddEvent(Event _event)
        {
            _context.Events.Add(_event);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateEvent(Event _event)
        {
            var existingEvent = await _context.Events.FindAsync(_event.Id);

            if (existingEvent != null)
            {
                existingEvent.Name = _event.Name;
                existingEvent.Description = _event.Description;
                existingEvent.StartTime = _event.StartTime;
                existingEvent.UserId = _event.UserId;

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveEvent(int eventId)
        {
            var _event = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            if (_event != null)
            {
                _context.Events.Remove(_event);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<byte[]> GetEventsCsvBytes()
        {
            var events = await _context.Events.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Events");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "StartTime";
                worksheet.Cell(1, 5).Value = "UserId";

                var row = 2;
                foreach (var _event in events)
                {
                    worksheet.Cell(row, 1).Value = _event.Id;
                    worksheet.Cell(row, 2).Value = _event.Name;
                    worksheet.Cell(row, 3).Value = _event.Description;
                    worksheet.Cell(row, 4).Value = _event.StartTime;
                    worksheet.Cell(row, 5).Value = _event.UserId;
                    row++;
                }

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    return memoryStream.ToArray();
                }
            }
        }

        public async Task<IEnumerable<Event>> SearchEvents(string searchTerm)
            => await Task.FromResult(_context.Events.Where(e => e.Name == searchTerm));

        public async Task<IEnumerable<Event>> SearchPartialEvents(string searchTerm)
        {
            var searchedEvents = await _context.Events
                .Where(e => e.Name.Contains(searchTerm))
                .ToListAsync();

            return searchedEvents;
        }

        public async Task AddUserToEvent(int eventId, int userId)
        {
            var @event = await _context.Events.Include(e => e.Participants).FirstOrDefaultAsync(e => e.Id == eventId);
            if (@event == null)
            {
                throw new ArgumentException("Event not found.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            if (@event.Participants.Any(p => p.Id == userId))
            {
                throw new InvalidOperationException("User is already part of the event.");
            }

            @event.Participants.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
