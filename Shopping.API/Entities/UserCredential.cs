using System.ComponentModel.DataAnnotations;

namespace Shopping.API.Entities
{
    public class UserCredential
    {
        [Required(ErrorMessage = "Username is required")]
        public string? userName { get; set; }
        [Required(ErrorMessage = "Passwaord is required")]
        public string? password { get; set; }
    }
}
