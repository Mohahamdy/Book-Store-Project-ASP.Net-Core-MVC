using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace Project.Models
{
    public class BookStoreContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {
        }

        public BookStoreContext() : base()
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrdersDetails { get; set; }
        public DbSet<Discount> Discounts { get; set; }


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // Add DbSet properties for Identity tables
        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<IdentityRole<int>> AspNetRoles { get; set; }
        public DbSet<IdentityUserRole<int>> AspNetUserRoles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.
                UseSqlServer("Data Source=.;Initial Catalog=BookStore;Integrated Security=True;Encrypt=False");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderDetails>().HasKey("Order_id", "Book_id");
            builder.Entity<Comment>().HasKey("user_id", "book_id");

            base.OnModelCreating(builder);
        }

    }
}
