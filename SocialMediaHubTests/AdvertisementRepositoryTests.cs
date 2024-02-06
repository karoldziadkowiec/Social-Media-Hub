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
    public class AdvertisementRepositoryTests
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task GetAllAdvertisements_ShouldReturnAllAdvertisements()
        {
            // Arrange
            var options = GetDbContextOptions("GetAllAdvertisements");

            using (var context = new AppDbContext(options))
            {
                var advertisementRepository = new AdvertisementRepository(context);

                context.Advertisements.AddRange(
                    new Advertisement { Id = 1, Title = "Ad1", Description = "Description 1", ImageURL = "url1", DestinationURL = "dest1", IsActive = true },
                    new Advertisement { Id = 2, Title = "Ad2", Description = "Description 2", ImageURL = "url2", DestinationURL = "dest2", IsActive = true }
                );
                context.SaveChanges();

                // Act
                var result = await advertisementRepository.GetAllAdvertisements();

                // Assert
                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task GetAdvertisement_ShouldReturnCorrectAdvertisement()
        {
            // Arrange
            var options = GetDbContextOptions("GetAdvertisement");

            using (var context = new AppDbContext(options))
            {
                var advertisementRepository = new AdvertisementRepository(context);

                context.Advertisements.Add(new Advertisement { Id = 1, Title = "Ad1", Description = "Description 1", ImageURL = "url1", DestinationURL = "dest1", IsActive = true });
                context.SaveChanges();

                // Act
                var result = await advertisementRepository.GetAdvertisement(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("Ad1", result.Title);
            }
        }

        [Fact]
        public async Task AddAdvertisement_ShouldAddAdvertisement()
        {
            // Arrange
            var options = GetDbContextOptions("AddAdvertisement");

            using (var context = new AppDbContext(options))
            {
                var advertisementRepository = new AdvertisementRepository(context);

                var advertisement = new Advertisement { Id = 1, Title = "Ad1", Description = "Description 1", ImageURL = "url1", DestinationURL = "dest1", IsActive = true };

                // Act
                await advertisementRepository.AddAdvertisement(advertisement);

                // Assert
                var result = await context.Advertisements.FirstOrDefaultAsync();
                Assert.NotNull(result);
                Assert.Equal("Ad1", result.Title);
            }
        }

        [Fact]
        public async Task UpdateAdvertisement_ShouldUpdateAdvertisement()
        {
            // Arrange
            var options = GetDbContextOptions("UpdateAdvertisement");

            using (var context = new AppDbContext(options))
            {
                var advertisementRepository = new AdvertisementRepository(context);

                context.Advertisements.Add(new Advertisement { Id = 1, Title = "Ad1", Description = "Description 1", ImageURL = "url1", DestinationURL = "dest1", IsActive = true });
                context.SaveChanges();

                var updatedAdvertisement = new Advertisement { Id = 1, Title = "Updated Ad", Description = "Updated Description", ImageURL = "updated-url", DestinationURL = "updated-dest", IsActive = false };

                // Act
                await advertisementRepository.UpdateAdvertisement(updatedAdvertisement);

                // Assert
                var result = await context.Advertisements.FirstOrDefaultAsync();
                Assert.NotNull(result);
                Assert.Equal("Updated Ad", result.Title);
            }
        }

        [Fact]
        public async Task RemoveAdvertisement_ShouldRemoveAdvertisement()
        {
            // Arrange
            var options = GetDbContextOptions("RemoveAdvertisement");

            using (var context = new AppDbContext(options))
            {
                var advertisementRepository = new AdvertisementRepository(context);

                context.Advertisements.Add(new Advertisement { Id = 1, Title = "Ad1", Description = "Description 1", ImageURL = "url1", DestinationURL = "dest1", IsActive = true });
                context.SaveChanges();

                // Act
                await advertisementRepository.RemoveAdvertisement(1);

                // Assert
                var result = await context.Advertisements.FirstOrDefaultAsync();
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetAdvertisementsCsvBytes_ShouldReturnCorrectCsvBytes()
        {
            // Arrange
            var options = GetDbContextOptions("GetAdvertisementsCsvBytes");

            using (var context = new AppDbContext(options))
            {
                var advertisementRepository = new AdvertisementRepository(context);

                context.Advertisements.AddRange(
                    new Advertisement { Id = 1, Title = "Ad1", Description = "Description 1", ImageURL = "url1", DestinationURL = "dest1", IsActive = true },
                    new Advertisement { Id = 2, Title = "Ad2", Description = "Description 2", ImageURL = "url2", DestinationURL = "dest2", IsActive = true }
                );
                context.SaveChanges();

                // Act
                var result = await advertisementRepository.GetAdvertisementsCsvBytes();

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Length > 0);
            }
        }
    }
}
