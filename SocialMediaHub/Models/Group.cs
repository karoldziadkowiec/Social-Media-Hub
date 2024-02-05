using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SocialMediaHub.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        public int Limit { get; set; }
    }
}
