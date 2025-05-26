namespace EDMN_EgitimPortali_WebAPI.DTOs.Comments
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; } 
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
