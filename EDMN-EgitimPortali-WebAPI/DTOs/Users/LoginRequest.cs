namespace EDMN_EgitimPortali_WebAPI.DTOs.Authentication
{
    public class LoginRequestDTO
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}