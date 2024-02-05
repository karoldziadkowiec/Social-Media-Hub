using System.ComponentModel.DataAnnotations;

namespace SocialMediaHub.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Location { get; set; }
        public int PhoneNumber { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; }
    }
}
