using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public interface IGroupRepository
    {
        Task<IQueryable<Group>> GetAllGroups();
        Task<Group> GetGroup(int userId);
        Task AddGroup(Group group);
        Task UpdateGroup(Group group);
        Task RemoveGroup(int groupId);
        Task<byte[]> GetGroupsCsvBytes();
        Task<IEnumerable<User>> GetUsersByGroupId(int groupId);
        Task<double> GetGroupFillPercentage(int groupId);
        Task<IEnumerable<Group>> GetGroupsByName();
        Task<Group> GetEmptyGroup();
        Task<IEnumerable<Group>> SearchGroups(string searchTerm);
        Task<IEnumerable<Group>> SearchPartialGroups(string searchTerm);
    }
}
