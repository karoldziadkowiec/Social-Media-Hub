using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaHub.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [ForeignKey("Organizer")]
        public int UserId { get; set; } //Organizer

        public ICollection<User> Participants { get; set; }
    }
}
