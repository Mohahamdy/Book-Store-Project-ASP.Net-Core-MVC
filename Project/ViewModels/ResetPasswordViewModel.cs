using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [Display(Name ="New Password")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*\\W)[A-Za-z\\d\\W]{8,}$", ErrorMessage = "Password must be at least 8 characters, at least one uppercase letter, one lowercase letter, one digit, and one non-letter/non-digit character.\r\n")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Not Matched Password.")]
        [Display(Name = "Confirm Passowrd")]
        public string ConfirmPassword { get; set; }
    }
}
