using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Project.Models;
using Project.ViewModels;
using System.Drawing.Printing;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Reflection.Metadata.BlobBuilder;

namespace Project.Controllers
{
    public class ShopController : Controller
    {
        private readonly BookStoreContext _context;

        public ShopController(BookStoreContext context)
        {
            _context = context;

        }
        // use IQueryable to can change of query and filter it like i want
        public IQueryable<Book> FilterBooks(List<string> categories, List<string> authors, int MiniPrice, int MaxPrice, string sort)
        {
            //here you have collection iquerable of books
            var pipline = _context.Books.AsQueryable();

            if (categories.Count > 0 && authors.Count > 0 && MiniPrice > 0)
            {

                pipline = pipline.Include(e => e.Category).Include(e => e.Author).Where(book => categories.Contains(book.Category.Name) && authors.Contains(book.Author.Name)
                && book.Price >= MiniPrice && book.Price <= MaxPrice);
            }
            else if (categories.Count > 0 && authors.Count > 0)
            {

                pipline = pipline.Include(e => e.Category).Include(e => e.Author).Where(book => categories.Contains(book.Category.Name) && authors.Contains(book.Author.Name));
            }
            else if (categories.Count > 0 && MiniPrice > 0 && MaxPrice < 1000)
            {
                pipline = pipline.Include(e => e.Category).Where(book => categories.Contains(book.Category.Name) && book.Price >= MiniPrice && book.Price <= MaxPrice);
            }
            else if (categories.Count > 0)
            {
                pipline = pipline.Include(e => e.Category).Where(book => categories.Contains(book.Category.Name));
            }
            else if (authors.Count > 0)
            {
                pipline = pipline.Include(e => e.Author).Where(book => authors.Contains(book.Author.Name));
            }
            else if (MiniPrice > 0)
            {
                pipline = pipline.Include(e => e.Author).Where(book => book.Price <= MaxPrice && book.Price >= MiniPrice);
            }
            //else {
            //    pipline = pipline.Include(e => e.Author).Where(book => book.Price <= MaxPrice);
            //}
            if (sort == "asc")
            {
                pipline = pipline.OrderBy(item => item.Price);
            }
            else if (sort == "dec")
            {
                pipline = pipline.OrderByDescending(item => item.Price);

            }
            return pipline.Include(e => e.Category).Include(e => e.Author);
        }

        public ViewResult CardsBooks(List<string> categories, List<string> authors, int MiniPrice, int MaxPrice, int? page, string sort)
        {

            int pageSize = 12; // Number of books per page
            int pageNumber = page ?? 1; // Current page number
            int skipCount = (pageNumber - 1) * pageSize; // Number of books to skip
            List<Book> filteredBooks = FilterBooks(categories, authors, MiniPrice, MaxPrice, sort).ToList();

            // Get total number of filtered books
            int totalBooks = filteredBooks.Count();

            // Paginate the filtered books
            List<Book> booksOnPage = filteredBooks.Skip(skipCount).Take(pageSize).ToList();

            // Calculate total number of pages
            int totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

            //Not compeleted
            ViewBag.TotalPages = totalPages;
            ListBooksAndNavigationVm vm = new ListBooksAndNavigationVm();
            vm.books = booksOnPage;
            vm.totalPages = totalPages;
            vm.pageNumber = pageNumber;
            vm.sort = sort;
            return View("CardsBooks", vm);
        }


        public IActionResult index(CategoriesAndAuthorsVM CAVM)
        {
            int pageSize = 12;
            int totalBooks = _context.Books.Count();
            int pageNumber = CAVM.pageNumber ?? 1;
            CAVM.totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);
            CAVM.pageNumber = pageNumber;
            CAVM.Categories = _context.Categories.Include(b => b.Books).ToList();
            CAVM.Authors = _context.Authors.Include(b => b.Books).ToList();
            CAVM.books = _context.Books.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return View("Shop", CAVM);


        }
    }
}
