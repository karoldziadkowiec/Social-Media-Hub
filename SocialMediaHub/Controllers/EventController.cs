using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaHub.Models;
using SocialMediaHub.Repositories;

namespace SocialMediaHub.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: /api/events
        [HttpGet]
        public async Task<IActionResult> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllEvents();
            return Ok(events);
        }

        // GET: /api/events/:id 
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventAsync(int eventId)
        {
            var _event = await _eventRepository.GetEvent(eventId);

            if (_event == null)
                return NotFound();

            return Ok(_event);
        }

        // POST: /api/events
        [HttpPost]
        public async Task<IActionResult> AddEventAsync([FromBody] Models.Event _event)
        {
            if (_event == null)
                return BadRequest();

            await _eventRepository.AddEvent(_event);

            return CreatedAtAction(nameof(GetEventAsync), new { eventId = _event.Id }, _event);
        }

        // PUT: /api/events/:id
        [HttpPut]
        public async Task<IActionResult> UpdateEventAsync(int eventId, [FromBody] Models.Event _event)
        {
            try
            {
                if (_event == null)
                    return BadRequest("Invalid event data");

                var existingEvent= await _eventRepository.GetEvent(eventId);

                if (existingEvent == null)
                    return NotFound();

                existingEvent.Name = _event.Name;
                existingEvent.Description = _event.Description;
                existingEvent.StartTime = _event.StartTime;
                existingEvent.UserId = _event.UserId;

                await _eventRepository.UpdateEvent(_event);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: /api/events/:id
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> RemoveEventAsync(int eventId)
        {
            try
            {
                var _event = await _eventRepository.GetEvent(eventId);

                if (_event == null)
                    return NotFound();

                await _eventRepository.RemoveEvent(eventId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: /api/events/csv
        [HttpGet("csv")]
        public async Task<IActionResult> GetEventsToCsvAsync()
        {
            try
            {
                var eventsCsvBytes = await _eventRepository.GetEventsCsvBytes();
                return File(eventsCsvBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "events.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{eventId}/user/{userId}")]
        public async Task<IActionResult> AddUserToEventAsync(int eventId, int userId)
        {
            try
            {
                await _eventRepository.AddUserToEvent(eventId, userId);
                return Ok("User added to the event successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
