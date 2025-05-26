using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDMN_EgitimPortali_WebAPI.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public string? VideoUrl { get; set; }

        public string? ThumbnailUrl { get; set; }

        [Required]
        public int OrderNo { get; set; }

        public string EducatorId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        public virtual ApplicationUser? Educator { get; set; }
    }
}