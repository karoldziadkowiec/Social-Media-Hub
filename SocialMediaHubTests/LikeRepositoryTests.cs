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
    public class LikeRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task GetAllLikes_ShouldReturnAllLikes()
        {
            // Arrange
            var options = GetDbContextOptions("GetAllLikes");

            using (var context = new AppDbContext(options))
            {
                var repository = new LikeRepository(context);

                context.Likes.AddRange(
                    new Like { Id = 1, IsLiked = true, CreationDate = DateTime.Now, UserId = 1, PostId = 1 },
                    new Like { Id = 2, IsLiked = false, CreationDate = DateTime.Now, UserId = 2, PostId = 1 }
                );
                context.SaveChanges();

                // Act
                var result = await repository.GetAllLikes();

                // Assert
                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task GetLike_ShouldReturnCorrectLike()
        {
            // Arrange
            var options = GetDbContextOptions("GetLike");

            using (var context = new AppDbContext(options))
            {
                var repository = new LikeRepository(context);

                context.Likes.AddRange(
                    new Like { Id = 1, IsLiked = true, CreationDate = DateTime.Now, UserId = 1, PostId = 1 },
                    new Like { Id = 2, IsLiked = false, CreationDate = DateTime.Now, UserId = 2, PostId = 1 }
                );
                context.SaveChanges();

                // Act
                var result = await repository.GetLike(1);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.IsLiked);
            }
        }

        [Fact]
        public async Task GetLikesCsvBytes_ShouldReturnCsvBytes()
        {
            // Arrange
            var options = GetDbContextOptions("GetLikesCsvBytes");

            using (var context = new AppDbContext(options))
            {
                var repository = new LikeRepository(context);

                context.Likes.AddRange(
                    new Like { Id = 1, IsLiked = true, CreationDate = DateTime.Now, UserId = 1, PostId = 1 },
                    new Like { Id = 2, IsLiked = false, CreationDate = DateTime.Now, UserId = 2, PostId = 1 }
                );
                context.SaveChanges();

                // Act
                var result = await repository.GetLikesCsvBytes();

                // Assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
            }
        }
    }
}
