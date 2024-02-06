using Microsoft.EntityFrameworkCore;
using SocialMediaHub.Database;
using SocialMediaHub.Models;
using SocialMediaHub.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaHubTests
{
    public class CommentRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task GetAllComments_ShouldReturnAllComments()
        {
            // Arrange
            var options = GetDbContextOptions("GetAllComments");

            using (var context = new AppDbContext(options))
            {
                var commentRepository = new CommentRepository(context);

                context.Comments.AddRange(
                    new Comment { Id = 1, Content = "Comment 1", CreationDate = DateTime.Now, UserId = 1, PostId = 1 },
                    new Comment { Id = 2, Content = "Comment 2", CreationDate = DateTime.Now, UserId = 2, PostId = 2 }
                );
                context.SaveChanges();

                // Act
                var result = await commentRepository.GetAllComments();

                // Assert
                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task GetComment_ShouldReturnCorrectComment()
        {
            // Arrange
            var options = GetDbContextOptions("GetComment");

            using (var context = new AppDbContext(options))
            {
                var commentRepository = new CommentRepository(context);

                context.Comments.AddRange(
                    new Comment { Id = 1, Content = "Comment 1", CreationDate = DateTime.Now, UserId = 1, PostId = 1 },
                    new Comment { Id = 2, Content = "Comment 2", CreationDate = DateTime.Now, UserId = 2, PostId = 2 }
                );
                context.SaveChanges();

                // Act
                var result = await commentRepository.GetComment(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Comment 1", result.Content);
            }
        }

        [Fact]
        public async Task AddComment_ShouldAddComment()
        {
            // Arrange
            var options = GetDbContextOptions("AddComment");

            using (var context = new AppDbContext(options))
            {
                var commentRepository = new CommentRepository(context);
                var comment = new Comment { Content = "New Comment", UserId = 1, PostId = 1 };

                // Act
                await commentRepository.AddComment(comment);

                // Assert
                Assert.NotEqual(0, comment.Id);
                var result = await context.Comments.FindAsync(comment.Id);
                Assert.NotNull(result);
                Assert.Equal("New Comment", result.Content);
            }
        }

        [Fact]
        public async Task EditComment_ShouldEditComment()
        {
            // Arrange
            var options = GetDbContextOptions("EditComment");

            using (var context = new AppDbContext(options))
            {
                var commentRepository = new CommentRepository(context);

                context.Comments.Add(new Comment { Id = 1, Content = "Original Comment", CreationDate = DateTime.Now, UserId = 1, PostId = 1 });
                context.SaveChanges();

                var editedComment = new Comment { Id = 1, Content = "Edited Comment", UserId = 2, PostId = 2 };

                // Act
                await commentRepository.EditComment(editedComment);

                // Assert
                var result = await context.Comments.FindAsync(1);
                Assert.NotNull(result);
                Assert.Equal("Edited Comment", result.Content);
                Assert.Equal(2, result.UserId);
                Assert.Equal(2, result.PostId);
            }
        }

        [Fact]
        public async Task RemoveComment_ShouldRemoveComment()
        {
            // Arrange
            var options = GetDbContextOptions("RemoveComment");

            using (var context = new AppDbContext(options))
            {
                var commentRepository = new CommentRepository(context);

                context.Comments.Add(new Comment { Id = 1, Content = "Comment 1", CreationDate = DateTime.Now, UserId = 1, PostId = 1 });
                context.SaveChanges();

                // Act
                await commentRepository.RemoveComment(1);

                // Assert
                var result = await context.Comments.FindAsync(1);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetCommentsCsvBytes_ShouldReturnCsvBytes()
        {
            // Arrange
            var options = GetDbContextOptions("GetCommentsCsvBytes");

            using (var context = new AppDbContext(options))
            {
                var commentRepository = new CommentRepository(context);

                context.Comments.AddRange(
                    new Comment { Id = 1, Content = "Comment 1", CreationDate = DateTime.Now, UserId = 1, PostId = 1 },
                    new Comment { Id = 2, Content = "Comment 2", CreationDate = DateTime.Now, UserId = 2, PostId = 2 }
                );
                context.SaveChanges();

                // Act
                var result = await commentRepository.GetCommentsCsvBytes();

                // Assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
            }
        }
    }
}
