using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaHub.Models;
using SocialMediaHub.Repositories;

namespace SocialMediaHub.Controllers
{
    [Route("api/friendships")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipRepository _friendshipRepository;

        public FriendshipController(IFriendshipRepository friendshipRepository)
        {
            _friendshipRepository = friendshipRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFriendships()
        {
            try
            {
                var result = await _friendshipRepository.GetAllFriendships();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{friendshipId}")]
        public async Task<IActionResult> GetFriendship(int friendshipId)
        {
            try
            {
                var result = await _friendshipRepository.GetFriendship(friendshipId);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFriendship(Friendship friendship)
        {
            try
            {
                await _friendshipRepository.AddFriendship(friendship);
                return Ok("Friendship added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{friendshipId}")]
        public async Task<IActionResult> RemoveFriendship(int friendshipId)
        {
            try
            {
                await _friendshipRepository.RemoveFriendship(friendshipId);
                return Ok("Friendship removed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("csv")]
        public async Task<IActionResult> GetFriendshipsCsv()
        {
            try
            {
                var friendshipsCsvBytes = await _friendshipRepository.GetFriendshipsCsvBytes();
                return File(friendshipsCsvBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "friendships.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
