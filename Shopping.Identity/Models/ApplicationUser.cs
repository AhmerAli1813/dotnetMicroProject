using Microsoft.AspNetCore.Identity;

namespace Shopping.Identity.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string? FatherName { get; set; }
    }
}
