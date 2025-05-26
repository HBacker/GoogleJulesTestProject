namespace EDMN_EgitimPortali_WebAPI.DTOs.Lessons
{
    public class LessonDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public int OrderNo { get; set; }
        public string EducatorId { get; set; }
    }
}
