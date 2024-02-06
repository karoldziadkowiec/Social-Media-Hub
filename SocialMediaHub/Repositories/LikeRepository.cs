using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using SocialMediaHub.Database;
using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;

        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Like>> GetAllLikes()
            => await Task.FromResult(_context.Likes.OrderBy(l => l.Id));

        public async Task<Like> GetLike(int likeId)
            => await _context.Likes.FirstOrDefaultAsync(l => l.Id == likeId);

        public async Task<byte[]> GetLikesCsvBytes()
        {
            var likes = await _context.Likes.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Posts");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "IsLiked";
                worksheet.Cell(1, 3).Value = "CreationDate";
                worksheet.Cell(1, 4).Value = "UserId";
                worksheet.Cell(1, 5).Value = "PostId";

                var row = 2;
                foreach (var like in likes)
                {
                    worksheet.Cell(row, 1).Value = like.Id;
                    worksheet.Cell(row, 2).Value = like.IsLiked;
                    worksheet.Cell(row, 3).Value = like.CreationDate;
                    worksheet.Cell(row, 4).Value = like.UserId;
                    worksheet.Cell(row, 5).Value = like.PostId;
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
