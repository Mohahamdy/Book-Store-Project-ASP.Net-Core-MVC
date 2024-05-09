using Project.Models;

namespace Project.ViewModels
{
    public class ListBooksAndNavigationVm
    {
        public List<Book> books { get; set; }
        public int totalPages { get; set; } 
        public int pageNumber { get; set; }

        public string sort {  get; set; }
    }
}
