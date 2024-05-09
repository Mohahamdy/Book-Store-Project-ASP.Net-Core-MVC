using System;

namespace Project.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public decimal SubTotal { get; set; }
        public int Quantity { get; set; }

        // Additional properties if needed
        
        public string BookName { get; set; }
        public string BookDescription { get; set; }
        public decimal BookPrice { get; set; }
        public int BookRate { get; set; }
        public string BookImage { get; set; }
        public int BookQuantity { get; set; }
        public int AdminId { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int DiscountId { get; set; }

        public DateTime OrderDate { get; set; }
        public TimeSpan OrderTime { get; set; }
        public decimal TotalPrice { get; set; }
        public int UserId { get; set; }
    }
}
