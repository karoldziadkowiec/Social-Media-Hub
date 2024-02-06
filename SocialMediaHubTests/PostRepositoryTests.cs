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
    public class PostRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task GetAllPosts_ShouldReturnAllPosts()
        {
            // Arrange
            var options = GetDbContextOptions("GetAllPosts");

            using (var context = new AppDbContext(options))
            {
                var postRepository = new PostRepository(context);

                context.Posts.AddRange(
                    new Post { Id = 1, Content = "Post 1", CreationDate = DateTime.Now, UserId = 1 },
                    new Post { Id = 2, Content = "Post 2", CreationDate = DateTime.Now, UserId = 2 }
                );
                context.SaveChanges();

                // Act
                var result = await postRepository.GetAllPosts();

                // Assert
                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task GetPost_ShouldReturnCorrectPost()
        {
            // Arrange
            var options = GetDbContextOptions("GetPost");

            using (var context = new AppDbContext(options))
            {
                var postRepository = new PostRepository(context);

                context.Posts.AddRange(
                    new Post { Id = 1, Content = "Post 1", CreationDate = DateTime.Now, UserId = 1 },
                    new Post { Id = 2, Content = "Post 2", CreationDate = DateTime.Now, UserId = 2 }
                );
                context.SaveChanges();

                // Act
                var result = await postRepository.GetPost(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Post 1", result.Content);
            }
        }

        [Fact]
        public async Task AddPost_ShouldAddPost()
        {
            // Arrange
            var options = GetDbContextOptions("AddPost");
            var expectedContent = "New Post Content";

            using (var context = new AppDbContext(options))
            {
                var postRepository = new PostRepository(context);
                var post = new Post { Content = expectedContent, UserId = 1 };

                // Act
                await postRepository.AddPost(post);

                // Assert
                var result = await context.Posts.FirstOrDefaultAsync(p => p.Content == expectedContent);
                Assert.NotNull(result);
                Assert.Equal(expectedContent, result.Content);
            }
        }

        [Fact]
        public async Task EditPost_ShouldEditPost()
        {
            // Arrange
            var options = GetDbContextOptions("EditPost");
            var originalContent = "Original Post Content";
            var editedContent = "Edited Post Content";

            using (var context = new AppDbContext(options))
            {
                var postRepository = new PostRepository(context);
                var post = new Post { Content = originalContent, UserId = 1 };
                await context.Posts.AddAsync(post);
                await context.SaveChangesAsync();

                // Act
                post.Content = editedContent;
                await postRepository.EditPost(post);

                // Assert
                var result = await context.Posts.FindAsync(post.Id);
                Assert.NotNull(result);
                Assert.Equal(editedContent, result.Content);
            }
        }

        [Fact]
        public async Task RemovePost_ShouldRemovePost()
        {
            // Arrange
            var options = GetDbContextOptions("RemovePost");

            using (var context = new AppDbContext(options))
            {
                var postRepository = new PostRepository(context);
                var post = new Post { Content = "Post to be removed", UserId = 1 };
                await context.Posts.AddAsync(post);
                await context.SaveChangesAsync();

                // Act
                await postRepository.RemovePost(post.Id);

                // Assert
                var result = await context.Posts.FindAsync(post.Id);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetPostsCsvBytes_ShouldReturnCsvBytes()
        {
            // Arrange
            var options = GetDbContextOptions("GetPostsCsvBytes");

            using (var context = new AppDbContext(options))
            {
                var postRepository = new PostRepository(context);

                context.Posts.AddRange(
                    new Post { Id = 1, Content = "Post 1", CreationDate = DateTime.Now, UserId = 1 },
                    new Post { Id = 2, Content = "Post 2", CreationDate = DateTime.Now, UserId = 2 }
                );
                await context.SaveChangesAsync();

                // Act
                var result = await postRepository.GetPostsCsvBytes();

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Length > 0);

                using (var memoryStream = new MemoryStream(result))
                using (var reader = new StreamReader(memoryStream))
                {
                    var csvContent = reader.ReadToEnd();
                }
            }
        }

        [Fact]
        public async Task AddCommentToPost_ShouldAddComment()
        {
            // Arrange
            var options = GetDbContextOptions("AddCommentToPost_ShouldAddComment");

            using (var context = new AppDbContext(options))
            {
                var repository = new PostRepository(context);

                var postId = 1;
                var userId = 1;
                var commentContent = "Test comment";

                context.Posts.Add(new Post { Id = postId, Content = "Test post content" });
                await context.SaveChangesAsync();

                // Act
                await repository.AddCommentToPost(postId, userId, commentContent);

                // Assert
                var comment = await context.Comments.FirstOrDefaultAsync();
                Assert.NotNull(comment);
                Assert.Equal(postId, comment.PostId);
                Assert.Equal(userId, comment.UserId);
                Assert.Equal(commentContent, comment.Content);
            }
        }


        [Fact]
        public async Task EditComment_ShouldEditComment()
        {
            // Arrange
            var options = GetDbContextOptions("EditComment_ShouldEditComment");

            using (var context = new AppDbContext(options))
            {
                var repository = new PostRepository(context);

                var commentId = 1;
                var originalContent = "Initial content";
                var updatedContent = "Updated content";

                context.Comments.Add(new Comment { Id = commentId, Content = originalContent });
                await context.SaveChangesAsync();

                var comment = new Comment { Id = commentId, Content = updatedContent };

                // Act
                await repository.EditComment(comment);

                // Assert
                var updatedComment = await context.Comments.FindAsync(commentId);
                Assert.NotNull(updatedComment);
                Assert.Equal(updatedContent, updatedComment.Content);
            }
        }

        [Fact]
        public async Task RemoveComment_ShouldRemoveComment()
        {
            // Arrange
            var options = GetDbContextOptions("RemoveComment_ShouldRemoveComment");

            using (var context = new AppDbContext(options))
            {
                var repository = new PostRepository(context);

                var postId = 1;
                var commentId = 1;

                context.Comments.Add(new Comment { Id = commentId, PostId = postId, Content = "Test comment content" });
                await context.SaveChangesAsync();

                // Act
                await repository.RemoveComment(postId, commentId);

                // Assert
                var removedComment = await context.Comments.FindAsync(commentId);
                Assert.Null(removedComment);
            }
        }

        [Fact]
        public async Task AddLikeToPost_ShouldAddLike()
        {
            // Arrange
            var options = GetDbContextOptions("AddLikeToPost_ShouldAddLike");

            using (var context = new AppDbContext(options))
            {
                var repository = new PostRepository(context);

                var postId = 1;
                var userId = 1;
                var likeId = 1;

                context.Posts.Add(new Post { Id = postId, Content = "Test post content" });
                await context.SaveChangesAsync();

                // Act
                await repository.AddLikeToPost(postId, userId, likeId);

                // Assert
                var like = await context.Likes.FirstOrDefaultAsync();
                Assert.NotNull(like);
                Assert.Equal(likeId, like.Id);
                Assert.True(like.IsLiked);
                Assert.Equal(postId, like.PostId);
                Assert.Equal(userId, like.UserId);
            }
        }

        [Fact]
        public async Task RemoveLike_ShouldRemoveLike()
        {
            // Arrange
            var options = GetDbContextOptions("RemoveLike_ShouldRemoveLike");

            using (var context = new AppDbContext(options))
            {
                var repository = new PostRepository(context);

                var postId = 1;
                var likeId = 1;

                context.Likes.Add(new Like { Id = likeId, PostId = postId });
                await context.SaveChangesAsync();

                // Act
                await repository.RemoveLike(postId, likeId);

                // Assert
                var removedLike = await context.Likes.FindAsync(likeId);
                Assert.Null(removedLike);
            }
        }
    }
}
