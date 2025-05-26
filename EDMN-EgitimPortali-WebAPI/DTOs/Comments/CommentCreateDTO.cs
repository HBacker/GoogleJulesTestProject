using System.ComponentModel.DataAnnotations;

namespace EDMN_EgitimPortali_WebAPI.DTOs.Comments
{
    public class CommentCreateDTO
    {
        [Required]
        public string Text { get; set; }
    }
}
