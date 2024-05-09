using Project.Models;

namespace Project.Repositories
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly BookStoreContext db;

        public OrderDetailsRepository(BookStoreContext db)
        {
            this.db = db;
        }

        public void Add(OrderDetails orderDetails)
        {
            db.Add(orderDetails);
        }

        public void DeleteByBookID(int id)
        {
            db.Remove(GetOrderDetailsByBookId(id));
        }

        public void DeleteByOrderID(int id)
        {
            db.Remove(GetOrderDetailsByOrderId(id));
        }

        public List<OrderDetails> GetAll()
        {
            return db.OrdersDetails.ToList();
        }

        public List<OrderDetails> GetOrderDetailsByBookId(int id)
        {
            return db.OrdersDetails.Where(o => o.Book_id == id).ToList();

        }
        public List<OrderDetails> GetOrderDetailsByOrderId(int id)
        {
            return db.OrdersDetails.Where(o => o.Order_id == id).ToList();
        }

        public OrderDetails GetOrderDetailsByOrderAndBookId(int user_id, int book_id)
        {
            return db.OrdersDetails.SingleOrDefault(o => o.Order_id == user_id && o.Book_id == book_id);
        }
        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(OrderDetails orderDetails)
        {
            db.Update(orderDetails);
        }
    }
}
