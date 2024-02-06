using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using SocialMediaHub.Database;
using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private readonly AppDbContext _context;

        public AdvertisementRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Advertisement>> GetAllAdvertisements()
            => await Task.FromResult(_context.Advertisements.OrderBy(a => a.Id));

        public async Task<Advertisement> GetAdvertisement(int advertisementId)
            => await _context.Advertisements.FirstOrDefaultAsync(a => a.Id == advertisementId);

        public async Task AddAdvertisement(Advertisement advertisement)
        {
            _context.Advertisements.Add(advertisement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAdvertisement(Advertisement updatedAdvertisement)
        {
            var existingAdvertisement = await _context.Advertisements.FindAsync(updatedAdvertisement.Id);

            if (existingAdvertisement == null)
            {
                throw new ArgumentException($"Advertisement with ID {updatedAdvertisement.Id} not found.");
            }

            existingAdvertisement.Title = updatedAdvertisement.Title;
            existingAdvertisement.Description = updatedAdvertisement.Description;
            existingAdvertisement.ImageURL = updatedAdvertisement.ImageURL;
            existingAdvertisement.DestinationURL = updatedAdvertisement.DestinationURL;
            existingAdvertisement.IsActive = updatedAdvertisement.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveAdvertisement(int advertisementId)
        {
            var advertisement = await _context.Advertisements.FindAsync(advertisementId);

            if (advertisement == null)
            {
                throw new ArgumentException($"Advertisement with ID {advertisementId} not found.");
            }

            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]> GetAdvertisementsCsvBytes()
        {
            var advertisements = await _context.Advertisements.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Advertisements");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Title";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "ImageURL";
                worksheet.Cell(1, 5).Value = "DestinationURL";
                worksheet.Cell(1, 6).Value = "IsActive";

                var row = 2;
                foreach (var advertisement in advertisements)
                {
                    worksheet.Cell(row, 1).Value = advertisement.Id;
                    worksheet.Cell(row, 2).Value = advertisement.Title;
                    worksheet.Cell(row, 3).Value = advertisement.Description;
                    worksheet.Cell(row, 4).Value = advertisement.ImageURL;
                    worksheet.Cell(row, 5).Value = advertisement.DestinationURL;
                    worksheet.Cell(row, 6).Value = advertisement.IsActive;
                    row++;
                }

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
