using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaHub.Models;
using SocialMediaHub.Repositories;

namespace SocialMediaHub.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: /api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsers();
            return Ok(users);
        }

        // GET: /api/users/:id 
        [HttpGet("/{userId}")]
        public async Task<IActionResult> GetUserAsync(int userId)
        {
            var user = await _userRepository.GetUser(userId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: /api/users
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            await _userRepository.AddUser(user);

            return CreatedAtAction(nameof(GetUserAsync), new { userId = user.Id }, user);
        }

        // DELETE: /api/users/:id
        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveUserAsync(int userId)
        {
            var user = await _userRepository.GetUser(userId);
            if (user == null)
                return NotFound();

            await _userRepository.RemoveUser(userId);

            return NoContent();
        }
    }
}
