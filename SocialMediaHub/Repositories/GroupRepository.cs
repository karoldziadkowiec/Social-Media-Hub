using Microsoft.EntityFrameworkCore;
using SocialMediaHub.Database;
using SocialMediaHub.Models;

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
    }
}
