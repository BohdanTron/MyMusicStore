using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ShippingDetails
    {
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Length must be from 3 to 10 characters")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter your country please")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Length must be from 3 to 10 characters")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Enter your city please")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Length must be from 3 to 10 characters")]
        public string City { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", 
            ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your phone please")]
        [StringLength(10, ErrorMessage = "Length must be equals 10 characters")]
        public string Phone { get; set; }

        public bool GiftWrap { get; set; }
    }
}
