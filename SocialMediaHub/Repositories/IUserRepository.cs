using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public interface IUserRepository
    {
        Task<IQueryable<User>> GetAllUsers();
        Task<User> GetUser(int userId);
        Task AddUser(User user);
        Task RemoveUser(int userId);
    }
}
