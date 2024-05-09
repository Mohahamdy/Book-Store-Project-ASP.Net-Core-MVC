using Project.Models;

namespace Project.Repositories
{
    public interface IOrderRepository
    {
        public List<Order> GetAll();


        public Order GetById(int id);
        public void Add(Order order);
        public void Update(Order order);
        public void DeleteByID(int id);

        public void Save();
    }
}