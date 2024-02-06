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
    public class GroupRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task GetAllGroups_ShouldReturnAllGroupsOrderedById()
        {
            // Arrange
            var options = GetDbContextOptions("GetAllGroups");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);

            context.Groups.AddRange(
                new Group { Id = 1, Name = "Group A", Limit = 10 },
                new Group { Id = 2, Name = "Group B", Limit = 15 }
            );

            context.SaveChanges();

            // Act
            var result = await groupRepository.GetAllGroups();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(1, result.First().Id);
            Assert.Equal(2, result.Last().Id);
        }

        [Fact]
        public async Task GetGroup_ShouldReturnCorrectGroup()
        {
            // Arrange
            var options = GetDbContextOptions("GetGroup");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);
            var expectedGroup = new Group { Id = 1, Name = "Group A", Limit = 10 };
            context.Groups.Add(expectedGroup);
            context.SaveChanges();

            // Act
            var result = await groupRepository.GetGroup(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedGroup.Id, result.Id);
            Assert.Equal(expectedGroup.Name, result.Name);
        }

        [Fact]
        public async Task AddGroup_ShouldAddNewGroup()
        {
            // Arrange
            var options = GetDbContextOptions("AddGroup");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);
            var newGroup = new Group { Id = 1, Name = "Group A", Limit = 10 };

            // Act
            await groupRepository.AddGroup(newGroup);

            // Assert
            var addedGroup = await context.Groups.FirstOrDefaultAsync(g => g.Id == 1);
            Assert.NotNull(addedGroup);
            Assert.Equal(newGroup.Id, addedGroup.Id);
            Assert.Equal(newGroup.Name, addedGroup.Name);
        }

        [Fact]
        public async Task UpdateGroup_ShouldUpdateExistingGroup()
        {
            // Arrange
            var options = GetDbContextOptions("UpdateGroup");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);
            var existingGroup = new Group { Id = 1, Name = "Group A", Limit = 10 };
            context.Groups.Add(existingGroup);
            context.SaveChanges();
            var updatedGroup = new Group { Id = 1, Name = "Updated Group A", Limit = 15 };

            // Act
            await groupRepository.UpdateGroup(updatedGroup);

            // Assert
            var result = await context.Groups.FirstOrDefaultAsync(g => g.Id == 1);
            Assert.NotNull(result);
            Assert.Equal(updatedGroup.Name, result.Name);
            Assert.Equal(updatedGroup.Limit, result.Limit);
        }

        [Fact]
        public async Task RemoveGroup_ShouldRemoveExistingGroup()
        {
            // Arrange
            var options = GetDbContextOptions("RemoveGroup");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);
            var existingGroup = new Group { Id = 1, Name = "Group A", Limit = 10 };
            context.Groups.Add(existingGroup);
            context.SaveChanges();

            // Act
            await groupRepository.RemoveGroup(1);

            // Assert
            var result = await context.Groups.FirstOrDefaultAsync(g => g.Id == 1);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetGroupsCsvBytes_ShouldReturnGroupsInCsvFormat()
        {
            // Arrange
            var options = GetDbContextOptions("GetGroupsCsvBytes");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);

            context.Groups.AddRange(
                new Group { Id = 1, Name = "Group A", Limit = 10 }
            );

            context.SaveChanges();

            // Act
            var result = await groupRepository.GetGroupsCsvBytes();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetUsersByGroupId_ShouldReturnUsersInGroup()
        {
            // Arrange
            var options = GetDbContextOptions("GetUsersByGroupId");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);

            context.Groups.Add(new Group { Id = 1, Name = "Group A", Limit = 10 });
            context.Users.AddRange(
               new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 },
               new User { Id = 2, Name = "Cristiano", Surname = "Ronaldo", Gender = "Male", Birthday = new DateTime(1985, 5, 20), Location = "Lisbona", PhoneNumber = 987654321, GroupId = 1 }
            );
            context.SaveChanges();

            // Act
            var result = await groupRepository.GetUsersByGroupId(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(1, result.First().Id);
            Assert.Equal(2, result.Last().Id);
        }

        [Fact]
        public async Task GetGroupFillPercentage_ShouldReturnCorrectFillPercentage()
        {
            // Arrange
            var options = GetDbContextOptions("GetGroupFillPercentage");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);

            context.Groups.Add(new Group { Id = 1, Name = "Group A", Limit = 10 });
            context.Users.AddRange(
               new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 },
               new User { Id = 2, Name = "Cristiano", Surname = "Ronaldo", Gender = "Male", Birthday = new DateTime(1985, 5, 20), Location = "Lisbona", PhoneNumber = 987654321, GroupId = 1 }
            );
            context.SaveChanges();

            // Act
            var result = await groupRepository.GetGroupFillPercentage(1);

            // Assert
            Assert.Equal(20.0, result);
        }

        [Fact]
        public async Task GetGroupsByName_ShouldReturnGroupsOrderedByName()
        {
            // Arrange
            var options = GetDbContextOptions("GetGroupsByName");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);

            context.Groups.AddRange(
                new Group { Id = 1, Name = "Group B", Limit = 10 },
                new Group { Id = 2, Name = "Group A", Limit = 15 }
            );

            context.SaveChanges();

            // Act
            var result = await groupRepository.GetGroupsByName();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(2, result.First().Id);
            Assert.Equal(1, result.Last().Id);
        }

        [Fact]
        public async Task GetEmptyGroup_ShouldReturnEmptyGroup()
        {
            // Arrange
            var options = GetDbContextOptions("GetEmptyGroup");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);

            context.Groups.AddRange(
                new Group { Id = 1, Name = "Group A", Limit = 10 },
                new Group { Id = 2, Name = "Group B", Limit = 5 }
            );

            context.Users.AddRange(
               new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 },
               new User { Id = 2, Name = "Cristiano", Surname = "Ronaldo", Gender = "Male", Birthday = new DateTime(1985, 5, 20), Location = "Lisbona", PhoneNumber = 987654321, GroupId = 1 }
            );

            context.SaveChanges();

            // Act
            var result = await groupRepository.GetEmptyGroup();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SearchGroups_ShouldReturnMatchingGroups()
        {
            // Arrange
            var options = GetDbContextOptions("SearchGroups");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);

            context.Groups.AddRange(
                new Group { Id = 1, Name = "Group A", Limit = 10 },
                new Group { Id = 2, Name = "Group B", Limit = 5 }
            );

            context.SaveChanges();

            // Act
            var result = await groupRepository.SearchGroups("Group A");

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }

        [Fact]
        public async Task SearchPartialGroups_ShouldReturnPartialMatchingGroups()
        {
            // Arrange
            var options = GetDbContextOptions("SearchPartialGroups");
            using var context = new AppDbContext(options);
            var groupRepository = new GroupRepository(context);

            context.Groups.AddRange(
                new Group { Id = 1, Name = "Group A", Limit = 10 },
                new Group { Id = 2, Name = "Group B", Limit = 5 }
            );

            context.SaveChanges();

            // Act
            var result = await groupRepository.SearchPartialGroups("A");

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }
    }
}
