using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class AuthorViewModel
    {
        [StringLength(maximumLength:40,MinimumLength =3,ErrorMessage ="Author length should be between 3 and 40 characters")]
        public string Name { get; set; }
    }
}
