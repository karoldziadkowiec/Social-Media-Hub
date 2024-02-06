using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaHub.Repositories;

namespace SocialMediaHub.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        // GET: /api/comments
        [HttpGet]
        public async Task<IActionResult> GetAllCommentsAsync()
        {
            var comments = await _commentRepository.GetAllComments();
            return Ok(comments);
        }

        // GET: /api/comments/:id 
        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetCommentAsync(int commentId)
        {
            var comment = await _commentRepository.GetComment(commentId);

            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        // GET: /api/comments/csv
        [HttpGet("csv")]
        public async Task<IActionResult> GetCommentsToCsvAsync()
        {
            try
            {
                var commentsCsvBytes = await _commentRepository.GetCommentsCsvBytes();
                return File(commentsCsvBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "comments.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
