using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using SocialMediaHub.Database;
using SocialMediaHub.Models;
using System.Reflection;
using System.Text;

namespace SocialMediaHub.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;

        public GroupRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Group>> GetAllGroups()
            => await Task.FromResult(_context.Groups.OrderBy(g => g.Id));

        public async Task<Group> GetGroup(int groupId)
            => await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);

        public async Task AddGroup(Group group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGroup(Group group)
        {
            var existingGroup = await _context.Groups.FindAsync(group.Id);

            if (existingGroup != null)
            {
                existingGroup.Name = group.Name;
                existingGroup.Limit = group.Limit;

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveGroup(int groupId)
        {
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<byte[]> GetGroupsCsvBytes()
        {
            var groups = await _context.Groups.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Groups");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Limit";

                var row = 2;
                foreach (var group in groups)
                {
                    worksheet.Cell(row, 1).Value = group.Id;
                    worksheet.Cell(row, 2).Value = group.Name;
                    worksheet.Cell(row, 3).Value = group.Limit;
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

        public async Task<IEnumerable<User>> GetUsersByGroupId(int groupId)
        {
            return await _context.Users
                .Where(u => u.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<double> GetGroupFillPercentage(int groupId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            double totalUsersCount = group.Limit;

            var groupUsersCount = await _context.Users.CountAsync(g => g.GroupId == groupId);

            if (totalUsersCount == 0)
                return 0.0;

            return (double)groupUsersCount / totalUsersCount * 100.0;
        }

        public async Task<IEnumerable<Group>> GetGroupsByName()
            => await Task.FromResult(_context.Groups.OrderBy(g => g.Name));

        public async Task<Group> GetEmptyGroup()
        {
            var emptyGroup = await _context.Groups
                .Where(g => g.Limit > 0 && !_context.Users.Any(g => g.GroupId == g.Id))
                .FirstOrDefaultAsync();

            return emptyGroup;
        }
        public async Task<IEnumerable<Group>> SearchGroups(string searchTerm)
            => await Task.FromResult(_context.Groups.Where(g => g.Name == searchTerm));

        public async Task<IEnumerable<Group>> SearchPartialGroups(string searchTerm)
        {
            var searchedUsers = await _context.Groups
                .Where(u => u.Name.Contains(searchTerm))
                .ToListAsync();

            return searchedUsers;
        }
    }
}
