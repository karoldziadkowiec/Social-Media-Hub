using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaHub.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
