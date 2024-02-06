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
    public class UserRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnAllUsersOrderedById()
        {
            // Arrange
            var options = GetDbContextOptions("GetAllUsers");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);

            context.Users.AddRange(
               new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 },
               new User { Id = 2, Name = "Cristiano", Surname = "Ronaldo", Gender = "Male", Birthday = new DateTime(1985, 5, 20), Location = "Lisbona", PhoneNumber = 987654321, GroupId = 2 }
           );

            context.SaveChanges();

            // Act
            var result = await userRepository.GetAllUsers();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(1, result.First().Id);
            Assert.Equal(2, result.Last().Id);
        }

        [Fact]
        public async Task GetUser_ShouldReturnCorrectUser()
        {
            // Arrange
            var options = GetDbContextOptions("GetUser");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);
            var expectedUser = new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 };
            context.Users.Add(expectedUser);
            context.SaveChanges();

            // Act
            var result = await userRepository.GetUser(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.Name, result.Name);
            Assert.Equal(expectedUser.Surname, result.Surname);
        }

        [Fact]
        public async Task AddUser_ShouldAddNewUser()
        {
            // Arrange
            var options = GetDbContextOptions("AddUser");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);
            var newUser = new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 };

            // Act
            await userRepository.AddUser(newUser);

            // Assert
            var addedUser = await context.Users.FirstOrDefaultAsync(u => u.Id == 1);
            Assert.NotNull(addedUser);
            Assert.Equal(newUser.Id, addedUser.Id);
            Assert.Equal(newUser.Name, addedUser.Name);
            Assert.Equal(newUser.Surname, addedUser.Surname);
        }

        [Fact]
        public async Task UpdateUser_ShouldUpdateExistingUser()
        {
            // Arrange
            var options = GetDbContextOptions("UpdateUser");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);
            var existingUser = new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 };
            context.Users.Add(existingUser);
            context.SaveChanges();

            var updatedUser = new User { Id = 1, Name = "Lionel", Surname = "Messi" };

            // Act
            await userRepository.UpdateUser(updatedUser);

            // Assert
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == 1);
            Assert.NotNull(user);
            Assert.Equal(updatedUser.Name, user.Name);
        }

        [Fact]
        public async Task RemoveUser_ShouldRemoveExistingUser()
        {
            // Arrange
            var options = GetDbContextOptions("RemoveUser");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);
            var existingUser = new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 };
            context.Users.Add(existingUser);
            context.SaveChanges();

            // Act
            await userRepository.RemoveUser(1);

            // Assert
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == 1);
            Assert.Null(user);
        }

        [Fact]
        public async Task GetUsersCsvBytes_ShouldReturnUsersInCsvFormat()
        {
            // Arrange
            var options = GetDbContextOptions("GetUsersInCsvFormat");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);

            context.Users.AddRange(
                new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 },
                new User { Id = 2, Name = "Cristiano", Surname = "Ronaldo", Gender = "Male", Birthday = new DateTime(1985, 5, 20), Location = "Lisbona", PhoneNumber = 987654321, GroupId = 2 }
            );

            context.SaveChanges();

            // Act
            var result = await userRepository.GetUsersCsvBytes();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetUsersByLocation_ShouldReturnUsersWithGivenLocation()
        {
            // Arrange
            var options = GetDbContextOptions("GetUsersByLocation");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);

            context.Users.AddRange(
               new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 },
                new User { Id = 2, Name = "Cristiano", Surname = "Ronaldo", Gender = "Male", Birthday = new DateTime(1985, 5, 20), Location = "Lisbona", PhoneNumber = 987654321, GroupId = 2 }
           );

            context.SaveChanges();

            // Act
            var result = await userRepository.GetUsersByLocation("Miami");

            // Assert
            Assert.Equal(1, result.Count());
            Assert.Equal("Leo", result.First().Name);
        }

        [Fact]
        public async Task GetUsersByGender_ShouldReturnUsersWithGivenGender()
        {
            // Arrange
            var options = GetDbContextOptions("GetUsersByGender");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);

            context.Users.AddRange(
               new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 },
                new User { Id = 2, Name = "Cristiano", Surname = "Ronaldo", Gender = "Male", Birthday = new DateTime(1985, 5, 20), Location = "Lisbona", PhoneNumber = 987654321, GroupId = 2 }
           );

            context.SaveChanges();

            // Act
            var result = await userRepository.GetUsersByGender("Male");

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Leo", result.First().Name);
            Assert.Equal("Cristiano", result.Last().Name);
        }

        [Fact]
        public async Task GetOldestUser_ShouldReturnOldestUser()
        {
            // Arrange
            var options = GetDbContextOptions("GetOldestUser");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);

            context.Users.AddRange(
               new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 },
                new User { Id = 2, Name = "Cristiano", Surname = "Ronaldo", Gender = "Male", Birthday = new DateTime(1985, 5, 20), Location = "Lisbona", PhoneNumber = 987654321, GroupId = 2 }
           );

            context.SaveChanges();

            // Act
            var result = await userRepository.GetOldestUser();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cristiano", result.Name);
        }

        [Fact]
        public async Task GetYoungestUser_ShouldReturnYoungestUser()
        {
            // Arrange
            var options = GetDbContextOptions("GetYoungestUser");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);

            context.Users.AddRange(
               new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 },
                new User { Id = 2, Name = "Cristiano", Surname = "Ronaldo", Gender = "Male", Birthday = new DateTime(1985, 5, 20), Location = "Lisbona", PhoneNumber = 987654321, GroupId = 2 }
           );

            context.SaveChanges();

            // Act
            var result = await userRepository.GetYoungestUser();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Leo", result.Name);
        }

        [Fact]
        public async Task SearchUsers_ShouldReturnUsersWithMatchingNameOrSurname()
        {
            // Arrange
            var options = GetDbContextOptions("SearchUsers");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);
            var _string = "Leo";

            context.Users.AddRange(
               new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 },
                new User { Id = 2, Name = "Cristiano", Surname = "Ronaldo", Gender = "Male", Birthday = new DateTime(1985, 5, 20), Location = "Lisbona", PhoneNumber = 987654321, GroupId = 2 }
           );

            context.SaveChanges();

            // Act
            var result = await userRepository.SearchUser(_string);

            // Assert
            Assert.Equal(1, result.Count());
            Assert.Equal("Leo", result.First().Name);
        }

        [Fact]
        public async Task SearchPartial_ShouldReturnUsersWithPartialNameOrSurname()
        {
            // Arrange
            var options = GetDbContextOptions("SearchPartial");
            using var context = new AppDbContext(options);
            var userRepository = new UserRepository(context);
            var partialString = "Mess";

            context.Users.AddRange(
               new User { Id = 1, Name = "Leo", Surname = "Messi", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 },
                new User { Id = 2, Name = "Cristiano", Surname = "Ronaldo", Gender = "Male", Birthday = new DateTime(1985, 5, 20), Location = "Lisbona", PhoneNumber = 987654321, GroupId = 2 }
           );

            context.SaveChanges();

            // Act
            var result = await userRepository.SearchPartial(partialString);

            // Assert
            Assert.Equal(1, result.Count());
            Assert.Equal("Leo", result.First().Name);
        }
    }
}