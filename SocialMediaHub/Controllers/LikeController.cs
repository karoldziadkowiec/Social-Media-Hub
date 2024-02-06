using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaHub.Repositories;

namespace SocialMediaHub.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;

        public LikeController(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        // GET: /api/likes
        [HttpGet]
        public async Task<IActionResult> GetAllLikesAsync()
        {
            var likes = await _likeRepository.GetAllLikes();
            return Ok(likes);
        }

        // GET: /api/likes/:id 
        [HttpGet("{likeId}")]
        public async Task<IActionResult> GetLikeAsync(int likeId)
        {
            var like = await _likeRepository.GetLike(likeId);

            if (like == null)
                return NotFound();

            return Ok(like);
        }

        // GET: /api/likes/csv
        [HttpGet("csv")]
        public async Task<IActionResult> GetCommentsToCsvAsync()
        {
            try
            {
                var likesCsvBytes = await _likeRepository.GetLikesCsvBytes();
                return File(likesCsvBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "likes.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
