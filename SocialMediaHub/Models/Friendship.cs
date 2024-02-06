using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialMediaHub.Models
{
    public class Friendship
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int User1Id { get; set; }

        [ForeignKey("User")]
        public int User2Id { get; set; }
    }
}
