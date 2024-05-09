using Project.Models;
using Project.ViewModels;

namespace Project.Repositories
{


    public class BookRepository : IBookRepository
    {
        BookStoreContext db;
        public BookRepository(BookStoreContext db)
        {
            this.db = db;
        }

        public void Add(Book book)
        {
            db.Add(book);
        }

        public void DeleteByID(int id)
        {
            db.Remove(GetById(id));
        }

        public List<Book> GetAll()
        {
            return db.Books.ToList();
        }

        //public BookDetailsVM GetBookDetails(int id)
        //{


        //    return bookvm;
        //}

        public Book GetById(int id)
        {
            return db.Books.FirstOrDefault(b => b.ID == id);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Book book)
        {
            db.Update(book);
        }
    }
}
