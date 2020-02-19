using System.ComponentModel.DataAnnotations;

namespace machine_api.Models.User
{
    public class UpdateModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter FirstName")]
        [DataType(DataType.Text)]
        [MaxLength(100), MinLength(1)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter LastName")]
        [DataType(DataType.Text)]
        [MaxLength(100), MinLength(1)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
    }
}
