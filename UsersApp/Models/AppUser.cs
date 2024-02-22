using Microsoft.AspNetCore.Identity;

namespace UsersApp.Models
{
    public class AppUser:IdentityUser
    {
            //public int? Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime LastLoginTime { get; set; }
            public DateTime RegistrationTime { get; set; }
            public bool IsActive { get; set; }
        }
    }
