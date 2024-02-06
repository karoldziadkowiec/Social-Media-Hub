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

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
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
    }
}
