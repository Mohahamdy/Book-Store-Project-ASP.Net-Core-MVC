using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class UniqueAttribute : ValidationAttribute
    {


        //check emp name must be unique(server side validation)
        protected override ValidationResult? IsValid
            (object? value, ValidationContext validationContext)
        {

            string name = value as string;
            Category CatFromRequest =
                (Category)validationContext.ObjectInstance;

            BookStoreContext context = validationContext.GetService<BookStoreContext>();

            Category CatFromDB = context.Categories.FirstOrDefault(e => e.Name == name);

            if (CatFromDB == null)
            {
                return ValidationResult.Success;
            }
            //logic
            return new ValidationResult("This Name Already exists");
        }
    }
}
