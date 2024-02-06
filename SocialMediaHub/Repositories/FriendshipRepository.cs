using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using SocialMediaHub.Database;
using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly AppDbContext _context;

        public FriendshipRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Friendship>> GetAllFriendships()
            => await Task.FromResult(_context.Friendships.OrderBy(f => f.Id));

        public async Task<Friendship> GetFriendship(int friendshipId)
            => await _context.Friendships.FirstOrDefaultAsync(f => f.Id == friendshipId);

        public async Task AddFriendship(Friendship friendship)
        {
            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFriendship(int friendshipId)
        {
            var friendship = await _context.Friendships.FirstOrDefaultAsync(f => f.Id == friendshipId);
            if (friendship != null)
            {
                _context.Friendships.Remove(friendship);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<byte[]> GetFriendshipsCsvBytes()
        {
            var friendships = await _context.Friendships.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Posts");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "User1Id";
                worksheet.Cell(1, 3).Value = "User2Id";

                var row = 2;
                foreach (var friendship in friendships)
                {
                    worksheet.Cell(row, 1).Value = friendship.Id;
                    worksheet.Cell(row, 2).Value = friendship.User1Id;
                    worksheet.Cell(row, 3).Value = friendship.User2Id;
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
