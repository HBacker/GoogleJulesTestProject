using System.ComponentModel.DataAnnotations;

namespace EDMN_EgitimPortali_WebAPI.DTOs.Lessons
{
    public class LessonCreateDTO
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public int OrderNo { get; set; }
    }
}
