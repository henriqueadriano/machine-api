using System.ComponentModel.DataAnnotations;

namespace machine_api.Models.User
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Please enter LastName")]
        [DataType(DataType.Text)]
        [MaxLength(100), MinLength(1)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter LastName")]
        [DataType(DataType.Text)]
        [MaxLength(100), MinLength(1)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100), MinLength(7)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Minimum 3 characters")]
        [DataType(DataType.Text)]
        [MaxLength(100), MinLength(3)]
        public string Password { get; set; }
    }
}
