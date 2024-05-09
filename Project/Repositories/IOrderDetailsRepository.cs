using Project.Models;

namespace Project.Repositories
{
    public interface IOrderDetailsRepository
    {

        public List<OrderDetails> GetAll();



        public void Add(OrderDetails orderDetails);
        public void Update(OrderDetails orderDetails);
        public void DeleteByBookID(int id);
        public void DeleteByOrderID(int id);
        public List<OrderDetails> GetOrderDetailsByBookId(int id);
        public List<OrderDetails> GetOrderDetailsByOrderId(int id);
        public OrderDetails GetOrderDetailsByOrderAndBookId(int order_id, int book_id);

        public void Save();
    }
}