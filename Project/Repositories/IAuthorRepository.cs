using Project.Models;

namespace Project.Repositories
{
    public interface IAuthorRepository
    {
        public List<Author> GetAll();


        public Author GetById(int id);
        public void Add(Author author);
        public void Update(Author author);
        public void DeleteByID(int id);

        public void Save();
    }
}