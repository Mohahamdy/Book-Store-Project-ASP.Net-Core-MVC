using Project.Models;

namespace Project.Repositories
{
    public interface ICategoryRepository
    {
        public List<Category> GetAll();
        public Category GetById(int id);
        public void Add(Category category);
        public void DeleteByID(int id);
        public void Update(Category category);
        public void Save();
    }
}