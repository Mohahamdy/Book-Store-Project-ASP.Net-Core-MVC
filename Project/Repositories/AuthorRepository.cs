using Project.Models;

namespace Project.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        BookStoreContext db;
        public AuthorRepository(BookStoreContext db)
        {
            this.db = db;
        }

        public List<Author> GetAll()
        {
            return db.Authors.ToList();
        }

        public Author GetById(int id) => db.Authors.FirstOrDefault(i => i.ID == id);

        public void Add(Author author)
        {
            db.Authors.Add(author);
        }

        public void Update(Author author)
        {
            db.Update(author);
        }
        public void DeleteByID(int id) => db.Authors.Remove(GetById(id));

        public void Save() => db.SaveChanges();
    }
}
