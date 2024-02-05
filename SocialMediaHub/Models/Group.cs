using System.ComponentModel.DataAnnotations;

namespace SocialMediaHub.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Limit { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
