using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EDMN_EgitimPortali_WebAPI.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } 

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }  

        [Required]
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
