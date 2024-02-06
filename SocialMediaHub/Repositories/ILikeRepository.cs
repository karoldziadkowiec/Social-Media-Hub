using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public interface ILikeRepository
    {
        Task<IQueryable<Like>> GetAllLikes();
        Task<Like> GetLike(int likeId);
        Task<byte[]> GetLikesCsvBytes();
    }
}
