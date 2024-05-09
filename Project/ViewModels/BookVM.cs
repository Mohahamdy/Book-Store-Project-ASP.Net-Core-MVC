using Project.Models;

namespace Project.ViewModels
{
    public class BookVM
    {
        //it is used for add & edit in dashboard -> to get list of Authors & Categories & ......

        public Book book { get; set; }
        public List<Author>? authors { get; set; }
        public List<ApplicationUser>? admins { get; set; }
        public List<Category>? categories { get; set; }
        public List<Discount>? discounts { get; set; }
    }
}
