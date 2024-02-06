using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using SocialMediaHub.Database;
using SocialMediaHub.Models;
using System.Reflection;
using System.Text;

namespace SocialMediaHub.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<User>> GetAllUsers()
            => await Task.FromResult(_context.Users.OrderBy(u => u.Id));

        public async Task<User> GetUser(int userId)
            => await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);

            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Surname = user.Surname;
                existingUser.Gender = user.Gender;
                existingUser.Birthday = user.Birthday;
                existingUser.Location = user.Location;
                existingUser.PhoneNumber = user.PhoneNumber;

                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<byte[]> GetUsersCsvBytes()
        {
            var users = await _context.Users.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Surname";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Birthday";
                worksheet.Cell(1, 6).Value = "Location";
                worksheet.Cell(1, 7).Value = "PhoneNumber";

                var row = 2;
                foreach (var user in users)
                {
                    worksheet.Cell(row, 1).Value = user.Id;
                    worksheet.Cell(row, 2).Value = user.Name;
                    worksheet.Cell(row, 3).Value = user.Surname;
                    worksheet.Cell(row, 4).Value = user.Gender;
                    worksheet.Cell(row, 5).Value = user.Birthday.ToShortDateString();
                    worksheet.Cell(row, 6).Value = user.Location;
                    worksheet.Cell(row, 7).Value = user.PhoneNumber;
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

        public async Task<IEnumerable<User>> GetUsersByLocation(string location)
             => await Task.FromResult(_context.Users.Where(u => u.Location == location));

        public async Task<IEnumerable<User>> GetUsersByGender(string gender)
             => await Task.FromResult(_context.Users.Where(u => u.Gender == gender));

        public async Task<User> GetOldestUser()
        {
            var oldestBirthday = await _context.Users.MinAsync(u => u.Birthday);

            return await _context.Users.FirstOrDefaultAsync(u => u.Birthday == oldestBirthday);
        }

        public async Task<User> GetYoungestUser()
        {
            var youngestBirthday = await _context.Users.MaxAsync(u => u.Birthday);

            return await _context.Users.FirstOrDefaultAsync(u => u.Birthday == youngestBirthday);
        }

        public async Task<IEnumerable<User>> SearchUser(string searchTerm)
            => await Task.FromResult(_context.Users.Where(u => u.Name == searchTerm || u.Surname == searchTerm));

        public async Task<IEnumerable<User>> SearchPartial(string searchTerm)
        {
            var searchedUsers = await _context.Users
                .Where(u => u.Name.Contains(searchTerm) || u.Surname.Contains(searchTerm))
                .ToListAsync();

            return searchedUsers;
        }
    }
}
