using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Enter your name please")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Enter your email please")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", 
            ErrorMessage ="Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your password please")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Length must be more than 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm your password please")]
        [Compare("Password", ErrorMessage = "Passwords do not confirm")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
