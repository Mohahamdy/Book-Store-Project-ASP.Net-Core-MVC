using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Discount
    {
        [Key]
        public int ID { get; set; }
        public int Percantage { get; set; }
        public DateTime Date { get; set; }

        public virtual List<Book> Books { set; get; } = new List<Book>();

    }
}
