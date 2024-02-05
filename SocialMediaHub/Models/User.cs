using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaHub.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(30)]
        public string Surname { get; set; }
        [MaxLength(10)]
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        [MaxLength(20)]
        public string Location { get; set; }
        public int PhoneNumber { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
    }
}
