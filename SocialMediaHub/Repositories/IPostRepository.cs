using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public interface IPostRepository
    {
        Task<IQueryable<Post>> GetAllPosts();
        Task<Post> GetPost(int postId);
        Task AddPost(Post post);
        Task EditPost(Post post);
        Task RemovePost(int postId);
        Task<byte[]> GetPostsCsvBytes();
        Task AddCommentToPost(int postId, int userId, string commentContent);
    }
}