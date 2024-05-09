using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class ApplicationUser : IdentityUser<int>
    {

        [MaxLength(50), MinLength(3)]
        public String FirstName { get; set; }
        [MaxLength(50), MinLength(3)]

        public String LastName { get; set; }

        public string image { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }

        public virtual List<Book> Books { set; get; } = new List<Book>();
        public virtual List<Comment> Comments { set; get; } = new List<Comment>();
        public virtual List<Order> Orders { set; get; } = new List<Order>();

    }
}
