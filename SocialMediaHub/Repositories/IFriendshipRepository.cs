using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public interface IFriendshipRepository
    {
        Task<IQueryable<Friendship>> GetAllFriendships();
        Task<Friendship> GetFriendship(int friendshipId);
        Task AddFriendship(Friendship friendship);
        Task RemoveFriendship(int friendshipId);
        Task<byte[]> GetFriendshipsCsvBytes();
    }
}
