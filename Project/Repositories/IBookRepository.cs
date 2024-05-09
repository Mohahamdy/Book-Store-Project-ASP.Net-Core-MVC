using Project.Models;
using Project.ViewModels;

namespace Project.Repositories
{
    public interface IBookRepository
    {
        //public BookDetailsVM GetBookDetails(int id);

        public List<Book> GetAll();


        public Book GetById(int id);
        public void Add(Book book);
        public void Update(Book book);
        public void DeleteByID(int id);

        public void Save();
    }
}