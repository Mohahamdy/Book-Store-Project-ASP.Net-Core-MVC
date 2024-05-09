using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{

    public class OrderDetails
    {
        [ForeignKey("order")]
        public int Order_id { get; set; }
        [ForeignKey("book")]
        public int Book_id { get; set; }
        [Column(TypeName = "money")]
        public decimal Sub_total { get; set; }

        public int Quantity { get; set; }

        public Order order { get; set; }
        public Book book { get; set; }

    }
}
