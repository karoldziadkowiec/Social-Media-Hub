using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialMediaHub.Database;
using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Post>> GetAllPosts()
            => await Task.FromResult(_context.Posts.OrderBy(p => p.Id));
        public async Task<Post> GetPost(int postId)
            => await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

        public async Task AddPost(Post post)
        {
            post.CreationDate = DateTime.Now;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task EditPost(Post post)
        {
            var existingPost = await _context.Posts.FindAsync(post.Id);

            if (existingPost != null)
            {
                existingPost.Content = post.Content;
                existingPost.CreationDate= post.CreationDate;
                existingPost.UserId = post.UserId;

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemovePost(int postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<byte[]> GetPostsCsvBytes()
        {
            var posts = await _context.Posts.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Posts");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Content";
                worksheet.Cell(1, 3).Value = "CreationDate";
                worksheet.Cell(1, 4).Value = "UserId";

                var row = 2;
                foreach (var post in posts)
                {
                    worksheet.Cell(row, 1).Value = post.Id;
                    worksheet.Cell(row, 2).Value = post.Content;
                    worksheet.Cell(row, 3).Value = post.CreationDate;
                    worksheet.Cell(row, 4).Value = post.UserId;
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
    }
}
