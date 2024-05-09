using Project.Models;

namespace Project.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookStoreContext db;

        public CategoryRepository(BookStoreContext db)
        {
            this.db = db;
        }
        public void Add(Category category)
        {
            db.Add(category);
        }

        public void DeleteByID(int id)
        {
            db.Remove(GetById(id));
        }

        public List<Category> GetAll()
        {
            return db.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return db.Categories.SingleOrDefault(c => c.ID == id);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Category category)
        {
            db.Update(category);
        }
    }
}
