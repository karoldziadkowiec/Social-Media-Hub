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
    }
}
