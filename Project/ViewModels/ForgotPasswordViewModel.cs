using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [RegularExpression("[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}", ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }
}
