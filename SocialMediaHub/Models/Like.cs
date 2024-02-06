using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaHub.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public bool IsLiked { get; set; }
        public DateTime CreationDate { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
    }
}
