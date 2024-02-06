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
    public class FriendshipRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task GetAllFriendships_ShouldReturnAllFriendships()
        {
            // Arrange
            var options = GetDbContextOptions("GetAllFriendships");

            using (var context = new AppDbContext(options))
            {
                var friendshipRepository = new FriendshipRepository(context);

                context.Friendships.AddRange(
                    new Friendship { Id = 1, User1Id = 1, User2Id = 2 },
                    new Friendship { Id = 2, User1Id = 2, User2Id = 3 }
                );
                context.SaveChanges();

                // Act
                var result = await friendshipRepository.GetAllFriendships();

                // Assert
                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task GetFriendship_ShouldReturnCorrectFriendship()
        {
            // Arrange
            var options = GetDbContextOptions("GetFriendship");

            using (var context = new AppDbContext(options))
            {
                var friendshipRepository = new FriendshipRepository(context);

                context.Friendships.AddRange(
                    new Friendship { Id = 1, User1Id = 1, User2Id = 2 },
                    new Friendship { Id = 2, User1Id = 2, User2Id = 3 }
                );
                context.SaveChanges();

                // Act
                var result = await friendshipRepository.GetFriendship(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal(1, result.User1Id);
                Assert.Equal(2, result.User2Id);
            }
        }

        [Fact]
        public async Task AddFriendship_ShouldAddFriendship()
        {
            // Arrange
            var options = GetDbContextOptions("AddFriendship");

            using (var context = new AppDbContext(options))
            {
                var friendshipRepository = new FriendshipRepository(context);
                var friendship = new Friendship { User1Id = 1, User2Id = 2 };

                // Act
                await friendshipRepository.AddFriendship(friendship);

                // Assert
                var result = await context.Friendships.FirstOrDefaultAsync(f => f.User1Id == 1 && f.User2Id == 2);
                Assert.NotNull(result);
            }
        }

        [Fact]
        public async Task RemoveFriendship_ShouldRemoveFriendship()
        {
            // Arrange
            var options = GetDbContextOptions("RemoveFriendship");

            using (var context = new AppDbContext(options))
            {
                var friendshipRepository = new FriendshipRepository(context);

                context.Friendships.AddRange(
                    new Friendship { Id = 1, User1Id = 1, User2Id = 2 },
                    new Friendship { Id = 2, User1Id = 2, User2Id = 3 }
                );
                context.SaveChanges();

                // Act
                await friendshipRepository.RemoveFriendship(1);

                // Assert
                var result = await context.Friendships.FirstOrDefaultAsync(f => f.Id == 1);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetFriendshipsCsvBytes_ShouldReturnCsvData()
        {
            // Arrange
            var options = GetDbContextOptions("GetFriendshipsCsvBytes");

            using (var context = new AppDbContext(options))
            {
                var friendshipRepository = new FriendshipRepository(context);

                context.Friendships.AddRange(
                    new Friendship { Id = 1, User1Id = 1, User2Id = 2 },
                    new Friendship { Id = 2, User1Id = 2, User2Id = 3 }
                );
                context.SaveChanges();

                // Act
                var result = await friendshipRepository.GetFriendshipsCsvBytes();

                // Assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
            }
        }
    }
}
