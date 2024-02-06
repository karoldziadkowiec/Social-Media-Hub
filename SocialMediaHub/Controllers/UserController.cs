using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaHub.Models;
using SocialMediaHub.Repositories;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;

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
        [HttpGet("{userId}")]
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
                return BadRequest("Invalid user data");

            try
            {
                await _userRepository.AddUser(user);

                return CreatedAtAction("GetUser", new { userId = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: /api/users/:id
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserAsync(int userId, [FromBody] User user)
        {
            try
            {
                if (user == null)
                    return BadRequest("Invalid user data");

                var existingUser = await _userRepository.GetUser(userId);

                if (existingUser == null)
                    return NotFound();

                existingUser.Name = user.Name;
                existingUser.Surname = user.Surname;
                existingUser.Gender = user.Gender;
                existingUser.Birthday = user.Birthday;
                existingUser.Location = user.Location;
                existingUser.PhoneNumber = user.PhoneNumber;

                await _userRepository.UpdateUser(existingUser);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: /api/users/:id
        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetUser(userId);

                if (user == null)
                    return NotFound();

                await _userRepository.RemoveUser(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: /api/users/csv
        [HttpGet("csv")]
        public async Task<IActionResult> GetUsersToCsvAsync()
        {
            try
            {
                var usersCsvBytes = await _userRepository.GetUsersCsvBytes();
                return File(usersCsvBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: /api/users/locaction/:location
        [HttpGet("location/{location}")]
        public async Task<IActionResult> GetUsersByLocationAsync(string location)
        {
            var users = await _userRepository.GetUsersByLocation(location);
            return Ok(users);
        }

        // GET: /api/users/gender/:gender
        [HttpGet("gender/{gender}")]
        public async Task<IActionResult> GetUsersByGenderAsync(string gender)
        {
            var users = await _userRepository.GetUsersByGender(gender);
            return Ok(users);
        }

        // GET: /api/users/oldest
        [HttpGet("oldest")]
        public async Task<IActionResult> GetOldestUserAsync()
        {
            var oldestUser = await _userRepository.GetOldestUser();
            return Ok(oldestUser);
        }

        // GET: /api/users/youngest
        [HttpGet("youngest")]
        public async Task<IActionResult> GetYoungestUserAsync()
        {
            var youngestUser = await _userRepository.GetYoungestUser();
            return Ok(youngestUser);
        }

        // GET: /api/users/search/:searchTerm
        [HttpGet("search/{searchTerm}")]
        public async Task<IActionResult> SearchUsersAsync(string searchTerm)
        {
            var searchedUsers = await _userRepository.SearchUsers(searchTerm);
            return Ok(searchedUsers);
        }

        // GET: /api/users/partial/:searchTerm
        [HttpGet("partial/{searchTerm}")]
        public async Task<IActionResult> SearchPartialUsersAsync(string searchTerm)
        {
            var searchedUsers = await _userRepository.SearchPartial(searchTerm);
            return Ok(searchedUsers);
        }
    }
}
