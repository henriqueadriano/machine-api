using System.ComponentModel.DataAnnotations;

namespace machine_api.Models.User
{
    public class AuthenticateModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
