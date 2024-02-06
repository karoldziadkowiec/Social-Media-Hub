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

        // POST: /api/comments
        [HttpPost]
        public async Task<IActionResult> AddCommentAsync([FromBody] Models.Comment comment)
        {
            if (comment == null)
                return BadRequest();

            await _commentRepository.AddComment(comment);

            return CreatedAtAction(nameof(GetCommentAsync), new { commentId = comment.Id }, comment);
        }

        // PUT: /api/comments/:id
        [HttpPut]
        public async Task<IActionResult> UpdateCommentAsync(int commentId, [FromBody] Models.Comment comment)
        {
            try
            {
                if (comment == null)
                    return BadRequest("Invalid comment data");

                var existingComment = await _commentRepository.GetComment(commentId);

                if (existingComment == null)
                    return NotFound();

                existingComment.Content = comment.Content;
                existingComment.CreationDate = comment.CreationDate;
                existingComment.UserId = comment.UserId;
                existingComment.PostId = comment.PostId;

                await _commentRepository.EditComment(comment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: /api/comments/:id
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> RemoveCommentAsync(int commentId)
        {
            try
            {
                var comment = await _commentRepository.GetComment(commentId);

                if (comment == null)
                    return NotFound();

                await _commentRepository.RemoveComment(commentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
