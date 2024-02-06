using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public interface ICommentRepository
    {
        Task<IQueryable<Comment>> GetAllComments();
        Task<Comment> GetComment(int commentId);
        Task AddComment(Comment comment);
        Task EditComment(Comment comment);
        Task RemoveComment(int commentId);
        Task<byte[]> GetCommentsCsvBytes();
    }
}