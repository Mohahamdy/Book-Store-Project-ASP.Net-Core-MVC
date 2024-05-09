using Project.Models;

namespace Project.Repositories
{
    public interface IDiscountRepository
    {
        public List<Discount> GetAll();
        public Discount GetById(int id);
        public void Add(Discount discount);
        public void DeleteByID(int id);
        public void Update(Discount discount);
        public void Save();
    }
}
