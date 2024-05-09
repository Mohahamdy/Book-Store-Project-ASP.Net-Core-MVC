using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Unique]
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }
        public bool IsAvailable { get; set; }

        public virtual List<Book> Books { set; get; } = new List<Book>();

    }
}
