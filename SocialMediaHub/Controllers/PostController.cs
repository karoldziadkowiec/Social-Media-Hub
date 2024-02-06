using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SocialMediaHub.Models;
using SocialMediaHub.Repositories;

namespace SocialMediaHub.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository _likeRepository;

        public PostController(IPostRepository postRepository, ICommentRepository commentRepository, ILikeRepository likeRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
        }

        // GET: /api/posts
        [HttpGet]
        public async Task<IActionResult> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllPosts();
            return Ok(posts);
        }

        // GET: /api/posts/:id 
        [HttpGet("{postId}")]
        public async Task<IActionResult> GetPostAsync(int postId)
        {
            var post = await _postRepository.GetPost(postId);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        // POST: /api/posts
        [HttpPost]
        public async Task<IActionResult> AddPostAsync([FromBody] Models.Post post)
        {
            if (post == null)
                return BadRequest();

            await _postRepository.AddPost(post);

            return CreatedAtAction(nameof(GetPostAsync), new { postId = post.Id }, post);
        }

        // PUT: /api/posts/:id
        [HttpPut]
        public async Task<IActionResult> UpdatePostAsync(int postId, [FromBody] Models.Post post)
        {
            try
            {
                if (post == null)
                    return BadRequest("Invalid post data");

                var existingPost = await _postRepository.GetPost(postId);

                if (existingPost == null)
                    return NotFound();

                existingPost.Content = post.Content;
                existingPost.CreationDate = post.CreationDate;
                existingPost.UserId = post.UserId;

                await _postRepository.EditPost(post);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: /api/posts/:id
        [HttpDelete("{postId}")]
        public async Task<IActionResult> RemovePostAsync(int postId)
        {
            try
            {
                var post = await _postRepository.GetPost(postId);

                if (post == null)
                    return NotFound();

                await _postRepository.RemovePost(postId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: /api/posts/csv
        [HttpGet("csv")]
        public async Task<IActionResult> GetPostsToCsvAsync()
        {
            try
            {
                var postsCsvBytes = await _postRepository.GetPostsCsvBytes();
                return File(postsCsvBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "posts.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{postId}/comments")]
        public async Task<IActionResult> AddCommentToPostAsync(int postId, int userId, string content)
        {
            try
            {
                await _postRepository.AddCommentToPost(postId, userId, content);
                return Ok("Comment added to post successfully.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{postId}/comments/{commentId}")]
        public async Task<IActionResult> EditCommentAsync(int postId, int commentId, Comment comment)
        {
            try
            {
                var existingComment = await _commentRepository.GetComment(commentId);
                if (existingComment == null || existingComment.PostId != postId)
                    return NotFound();

                await _postRepository.EditComment(comment);
                return Ok("Comment updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{postId}/comments/{commentId}")]
        public async Task<IActionResult> RemoveCommentAsync(int postId, int commentId)
        {
            try
            {
                var existingComment = await _commentRepository.GetComment(commentId);
                if (existingComment == null || existingComment.PostId != postId)
                    return NotFound();

                await _postRepository.RemoveComment(postId, commentId);
                return Ok("Comment removed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{postId}/likes")]
        public async Task<IActionResult> AddLikeToPost(int postId, int userId, int likeId)
        {
            try
            {
                await _postRepository.AddLikeToPost(postId, userId, likeId);
                return Ok("Like added to post successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{postId}/likes/{likeId}")]
        public async Task<IActionResult> RemoveLike(int postId, int likeId)
        {
            try
            {
                await _postRepository.RemoveLike(postId, likeId);
                return Ok("Like removed from post successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
