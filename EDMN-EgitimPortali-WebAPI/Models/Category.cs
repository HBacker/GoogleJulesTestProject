using EDMN_EgitimPortali_WebAPI.Models;
using System.ComponentModel.DataAnnotations;

public class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    public ICollection<CourseCategory> CourseCategories { get; set; } 
}
