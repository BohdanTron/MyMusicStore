using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter your email please")]
        public string NameOrEmail { get; set; }

        [Required(ErrorMessage = "Enter your password please")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Length must be more than 6 characters")]

        public string Password { get; set; }
    }
}
