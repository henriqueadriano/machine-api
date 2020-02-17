using System.ComponentModel.DataAnnotations;

namespace machine_api.Models.User
{
    public class UserModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
