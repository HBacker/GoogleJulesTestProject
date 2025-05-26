using Microsoft.AspNetCore.Identity;

namespace EDMN_EgitimPortali_WebAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserType { get; set; } 
       
    }
}
