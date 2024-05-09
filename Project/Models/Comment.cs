
using Project.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Comment
    {
        public Comment()
        {

        }
        public Comment(CommentVM c)
        {
            comment = c.comment;
            rate = c.rate;
            user_id = c.user_id;
            book_id = c.book_id;
            Date = DateTime.Now;
            IsAvailable = c.IsAvailable;
        }

        public string comment { get; set; }
        public decimal? rate { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("user")]
        public int user_id { get; set; }

        [ForeignKey("book")]
        public int book_id { get; set; }
        public bool IsAvailable { get; set; }
        public ApplicationUser user { get; set; }
        public Book book { get; set; }


    }
}