using Project.Models;

namespace Project.ViewModels
{
    public class HomeBooksVM
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }


        public decimal Price { get; set; }

        public decimal? Rate { get; set; }
        public bool IsAvailable { get; set; }

        public string? Image { get; set; }
        public int Quantity { get; set; }
        public string Author { get; set; }

        public string Category { get; set; }


        public int? Discount { get; set; }

    }
}
