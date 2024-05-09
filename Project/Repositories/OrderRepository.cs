using Project.Models;

namespace Project.Repositories
{
    public class OrderRepository : IOrderRepository
    {

        BookStoreContext db;
        public OrderRepository(BookStoreContext db)
        {
            this.db = db;
        }
        public void Add(Order order)
        {
            db.Add(order);
        }

        public void DeleteByID(int id)
        {
            db.Remove(GetById(id));
        }

        public List<Order> GetAll()
        {
            return db.Orders.ToList();
        }

        public Order GetById(int id)
        {
            return db.Orders.FirstOrDefault(o => o.ID == id);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Order order)
        {
            db.Update(order);
        }
    }
}
