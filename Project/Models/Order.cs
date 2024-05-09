using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        [Column(TypeName = "money")]
        public decimal Total_Price { get; set; }

        [ForeignKey("User")]
        public int user_id { get; set; }

        public ApplicationUser User { get; set; }

        public virtual List<OrderDetails> OrderDetails { set; get; } = new List<OrderDetails>();

    }
}
