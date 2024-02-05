using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public interface IGroupRepository
    {
        Task<IQueryable<Group>> GetAllGroups();
        Task<Group> GetGroup(int userId);
        Task AddGroup(Group group);
    }
}
