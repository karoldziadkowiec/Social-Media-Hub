using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public interface ICommentRepository
    {
        Task<IQueryable<Comment>> GetAllComments();
        Task<Comment> GetComment(int commentId);
        Task<byte[]> GetCommentsCsvBytes();
    }
}