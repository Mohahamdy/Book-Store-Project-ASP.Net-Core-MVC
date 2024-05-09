using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class addRoleViewModel
    {
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Please Select any Role.")]
        public int roleID { get; set; }
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Please Select any User.")]
        public int userID { get; set; }
    }
}
