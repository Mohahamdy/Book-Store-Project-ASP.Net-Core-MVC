using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required]
        [MaxLength(50,ErrorMessage ="First name Should be less than 50 characters")]
        [MinLength(3,ErrorMessage = "First name Should be more than 3 characters")]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Last name Should be less than 50 characters")]
        [MinLength(3, ErrorMessage = "Last name Should be more than 3 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}",ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*\\W)[A-Za-z\\d\\W]{8,}$",ErrorMessage = "Password must be at least 8 characters, at least one uppercase letter, one lowercase letter, one digit, and one non-letter/non-digit character.\r\n")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Not Matched Password.")]
        [Display(Name ="Confirm Passowrd")]
        public string ConfirmPassord { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [RegularExpression("^(010|011|015)\\d{8}$",ErrorMessage = "Please enter a valid Egyptian phone number starting with 010, 011, or 015 followed by 8 digits.")]
        public string Phone { get; set; }
    }
}
