using Microsoft.EntityFrameworkCore;
using SocialMediaHub.Models;

namespace SocialMediaHub.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
