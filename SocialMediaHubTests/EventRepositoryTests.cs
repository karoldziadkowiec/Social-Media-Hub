using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaHub.Controllers;
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
    public class EventRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnAllEvents()
        {
            // Arrange
            var options = GetDbContextOptions("GetAllEvents");
            using var context = new AppDbContext(options);
            var eventRepository = new EventRepository(context);

            context.Events.AddRange(
                new Event { Id = 1, Name = "Event 1", Description = "desc1", StartTime = DateTime.Now, UserId = 1 },
                new Event { Id = 2, Name = "Event 2", Description = "desc2", StartTime = DateTime.Now, UserId = 1 }
            );
            context.SaveChanges();

            // Act
            var result = await eventRepository.GetAllEvents();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetEvent_ShouldReturnEventById()
        {
            // Arrange
            var options = GetDbContextOptions("GetEvent");
            using var context = new AppDbContext(options);
            var eventRepository = new EventRepository(context);

            var testEvent = new Event { Id = 1, Name = "Test Event", Description = "Test Description", StartTime = DateTime.Now, UserId = 1 };
            context.Events.Add(testEvent);
            context.SaveChanges();

            // Act
            var result = await eventRepository.GetEvent(testEvent.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEvent.Name, result.Name);
            Assert.Equal(testEvent.Description, result.Description);
            Assert.Equal(testEvent.StartTime, result.StartTime);
            Assert.Equal(testEvent.UserId, result.UserId);
        }

        [Fact]
        public async Task AddEvent_ShouldAddNewEvent()
        {
            // Arrange
            var options = GetDbContextOptions("AddEvent");
            using var context = new AppDbContext(options);
            var eventRepository = new EventRepository(context);

            var newEvent = new Event { Name = "New Event", Description = "New Description", StartTime = DateTime.Now, UserId = 1 };

            // Act
            await eventRepository.AddEvent(newEvent);

            // Assert
            var result = await context.Events.FirstOrDefaultAsync(e => e.Name == "New Event");
            Assert.NotNull(result);
        }

        [Fact]
        public async Task UpdateEvent_ShouldUpdateExistingEvent()
        {
            // Arrange
            var options = GetDbContextOptions("UpdateEvent");
            using var context = new AppDbContext(options);
            var eventRepository = new EventRepository(context);

            var testEvent = new Event { Id = 1, Name = "Test Event", Description = "Test Description", StartTime = DateTime.Now, UserId = 1 };
            context.Events.Add(testEvent);
            context.SaveChanges();

            var updatedEvent = new Event { Id = 1, Name = "Updated Event", Description = "Updated Description", StartTime = DateTime.Now.AddDays(1), UserId = 2 };

            // Act
            await eventRepository.UpdateEvent(updatedEvent);

            // Assert
            var result = await context.Events.FindAsync(1);
            Assert.NotNull(result);
            Assert.Equal(updatedEvent.Name, result.Name);
            Assert.Equal(updatedEvent.Description, result.Description);
            Assert.Equal(updatedEvent.StartTime, result.StartTime);
            Assert.Equal(updatedEvent.UserId, result.UserId);
        }

        [Fact]
        public async Task RemoveEvent_ShouldRemoveEventById()
        {
            // Arrange
            var options = GetDbContextOptions("RemoveEvent");
            using var context = new AppDbContext(options);
            var eventRepository = new EventRepository(context);

            var testEvent = new Event { Id = 1, Name = "Test Event", Description = "Test Description", StartTime = DateTime.Now, UserId = 1 };
            context.Events.Add(testEvent);
            context.SaveChanges();

            // Act
            await eventRepository.RemoveEvent(testEvent.Id);

            // Assert
            var result = await context.Events.FindAsync(1);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetEventsCsvBytes_ShouldReturnBytes()
        {
            // Arrange
            var options = GetDbContextOptions("GetEventsCsvBytes");
            using var context = new AppDbContext(options);
            var eventRepository = new EventRepository(context);

            context.Events.AddRange(
                new Event { Id = 1, Name = "Event 1", Description = "desc1", StartTime = DateTime.Now, UserId = 1 },
                new Event { Id = 2, Name = "Event 2", Description = "desc2", StartTime = DateTime.Now, UserId = 1 }
            );
            context.SaveChanges();

            // Act
            var result = await eventRepository.GetEventsCsvBytes();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public async Task SearchEvents_ShouldReturnMatchingEvents()
        {
            // Arrange
            var options = GetDbContextOptions("SearchEvents");
            using var context = new AppDbContext(options);
            var eventRepository = new EventRepository(context);

            context.Events.AddRange(
                new Event { Id = 1, Name = "Event 1", Description = "desc1", StartTime = DateTime.Now, UserId = 1 },
                new Event { Id = 2, Name = "Event 2", Description = "desc2", StartTime = DateTime.Now, UserId = 1 },
                new Event { Id = 3, Name = "Event 3", Description = "desc3", StartTime = DateTime.Now, UserId = 2 }
            );
            context.SaveChanges();

            // Act
            var result = await eventRepository.SearchEvents("Event 1");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Event 1", result.First().Name);
        }

        [Fact]
        public async Task SearchPartialEvents_ShouldReturnPartialMatchingEvents()
        {
            // Arrange
            var options = GetDbContextOptions("SearchPartialEvents");
            using var context = new AppDbContext(options);
            var eventRepository = new EventRepository(context);

            context.Events.AddRange(
                new Event { Id = 1, Name = "Event 1", Description = "desc1", StartTime = DateTime.Now, UserId = 1 },
                new Event { Id = 2, Name = "Event 2", Description = "desc2", StartTime = DateTime.Now, UserId = 1 }
            );
            context.SaveChanges();

            // Act
            var result = await eventRepository.SearchPartialEvents("Event");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddUserToEvent_ShouldAddUserToEvent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "AddUserToEvent_ShouldAddUserToEvent")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var userRepository = new UserRepository(context);
                var eventRepository = new EventRepository(context);
                var controller = new EventController(eventRepository);

                var user = new User { Id = 1, Name = "John", Surname = "Doe", Gender = "Male", Birthday = new DateTime(1990, 1, 15), Location = "Miami", PhoneNumber = 123456789, GroupId = 1 };
                var @event = new Event { Id = 1, Name = "Test Event", Description = "Test Description", StartTime = DateTime.Now };

                await context.Users.AddAsync(user);
                await context.Events.AddAsync(@event);
                await context.SaveChangesAsync();

                // Act
                var result = await controller.AddUserToEventAsync(@event.Id, user.Id) as OkObjectResult;

                // Assert
                Assert.NotNull(result);
                Assert.Equal("User added to the event successfully.", result.Value);

                var updatedEvent = await eventRepository.GetEvent(@event.Id);
                Assert.Single(updatedEvent.Participants);
                Assert.Equal(user.Id, updatedEvent.Participants.First().Id);
            }
        }
    }
}
