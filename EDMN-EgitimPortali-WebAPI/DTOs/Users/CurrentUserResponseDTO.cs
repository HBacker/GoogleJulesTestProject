namespace EDMN_EgitimPortali_WebAPI.DTOs.Authentication
{
    public class CurrentUserResponseDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
    }
}