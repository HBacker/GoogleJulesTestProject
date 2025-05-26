using System.ComponentModel.DataAnnotations;

namespace EDMN_EgitimPortali_WebAPI.DTOs
{
    public class CreateCategoryDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
