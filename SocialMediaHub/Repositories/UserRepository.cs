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

        public async Task<IEnumerable<User>> GetUsersInCsvFormat()
        {
            var csvBuilder = new StringBuilder();

            csvBuilder.AppendLine("Id, Name, Surname, Gender, Birthday, Location, PhoneNumber");

            foreach (var user in _context.Users)
            {
                csvBuilder.AppendLine($"{user.Id}, {user.Name}, {user.Surname}, {user.Gender}, {user.Birthday},{user.Location},{user.PhoneNumber}");
            }

            return await _context.Users.ToListAsync();
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

        public async Task<IEnumerable<User>> SearchUsers(string searchTerm)
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
