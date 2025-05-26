using System.ComponentModel.DataAnnotations;

namespace EDMN_EgitimPortali_WebAPI.DTOs
{
    public class CourseUpdateDTO
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string? Content { get; set; }


        public string? PhotoUrl { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "En az bir kategori seçilmelidir.")]
        public List<int> CategoryIds { get; set; } 
    }
}