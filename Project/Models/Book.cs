using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Book
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public decimal? Rate { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "Quantity is Required")]
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }

        [ForeignKey("Admin")]
        public int Admin_id { get; set; }


        [ForeignKey("Author")]
        public int Author_id { get; set; }

        [ForeignKey("Category")]
        public int Category_id { get; set; }


        [ForeignKey("Discount")]
        public int? Discount_id { get; set; }
        public ApplicationUser? Admin { get; set; }

        public Author? Author { get; set; }

        public Category? Category { get; set; }
        public Discount? Discount { get; set; }
        public virtual List<Comment>? Comments { set; get; } = new List<Comment>();
        public virtual List<OrderDetails>? OrderDetails { set; get; } = new List<OrderDetails>();

    }
}
