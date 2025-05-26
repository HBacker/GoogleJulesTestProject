using System.Collections.Generic;
using EDMN_EgitimPortali_WebAPI.DTOs.Lessons;

namespace EDMN_EgitimPortali_WebAPI.DTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? PhotoUrl { get; set; }
        public List<LessonDTO> Lessons { get; set; }

        public List<int> CategoryIds { get; set; }

    }
}