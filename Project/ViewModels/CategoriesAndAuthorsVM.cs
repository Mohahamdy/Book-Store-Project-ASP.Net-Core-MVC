using Project.Models;

namespace Project.ViewModels
{
    public class CategoriesAndAuthorsVM
    {
        public List<Category> Categories { get; set; }
        public List<Author> Authors { get; set; }

        public List<Book> books { get; set; }

        public int totalPages { get; set; }
        public int? pageNumber { get; set; }
    }
}
