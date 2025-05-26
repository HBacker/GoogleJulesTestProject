using System;
using System.ComponentModel.DataAnnotations;

namespace EDMN_EgitimPortali_WebAPI.Models
{
    public class WatchedContent
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int LessonId { get; set; }

        public DateTime WatchedDate { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Lesson Lesson { get; set; }
    }
}
