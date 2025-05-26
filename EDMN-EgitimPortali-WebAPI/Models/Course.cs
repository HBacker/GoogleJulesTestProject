using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDMN_EgitimPortali_WebAPI.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string? Content { get; set; }

        public string? PhotoUrl { get; set; }

        public string? EducatorId { get; set; }

        public virtual ApplicationUser? Educator { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public List<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();
    }
}