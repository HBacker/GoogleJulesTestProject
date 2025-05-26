using System.ComponentModel.DataAnnotations;

namespace EDMN_EgitimPortali_WebAPI.DTOs.Authentication
{
    public class RegisterRequestDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Required]
        [RegularExpression("Student|Educator", ErrorMessage = "UserType(Rol) 'Student' veya 'Educator' olması zorunludur.")]
        public string UserType { get; set; }
    }
}
