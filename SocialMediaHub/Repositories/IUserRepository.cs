using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public interface IUserRepository
    {
        Task<IQueryable<User>> GetAllUsers();
        Task<User> GetUser(int userId);
        Task AddUser(User user);
        Task UpdateUser(User user);
        Task RemoveUser(int userId);
        Task<IEnumerable<User>> GetUsersInCsvFormat();
        Task<IEnumerable<User>> GetUsersByLocation(string location);
        Task<IEnumerable<User>> GetUsersByGender(string gender);
        Task<User> GetOldestUser();
        Task<User> GetYoungestUser();
        Task<IEnumerable<User>> SearchUsers(string searchTerm);
        Task<IEnumerable<User>> SearchPartial(string searchTerm);
    }
}