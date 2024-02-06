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
        // Comment
        Task AddCommentToPost(int postId, int userId, string commentContent);
        Task EditComment(Comment comment);
        Task RemoveComment(int postId, int commentId);
        // Like
        Task AddLikeToPost(int postId, int userId, int likeId);
        Task RemoveLike(int postId, int likeId);
    }
}