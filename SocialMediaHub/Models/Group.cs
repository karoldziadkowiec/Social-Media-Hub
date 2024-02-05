using System.ComponentModel.DataAnnotations;

namespace SocialMediaHub.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(10)]
        public int Limit { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
