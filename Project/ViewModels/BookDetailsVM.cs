
using Project.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.ViewModels
{
	public class BookDetailsVM
	{
		public int ID { get; set; }
		public string Name { get; set; }

		public string Description { get; set; }
        public int book_id { get; set; }

        public decimal Price { get; set; }

		public decimal? Rate { get; set; }
		public bool IsAvailable { get; set; }

		public string? Image { get; set; }
		public int Quantity { get; set; }
		public Author Author { get; set; }
		public List<BookDetailsVM> authorBooks { get; set; }  //books from the same author
		public Category Category { get; set; }
		public int categoryID { get; set; }
		public List<BookDetailsVM> categoryBooks { get; set; }  //books from the same category
		public Discount? Discount { get; set; }
		public List<CommentVM> Comments { set; get; }
		public int commentsNum { get; set; }
        public ApplicationUser Admin { get; set; }


    }
}