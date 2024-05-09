using Project.Models;

namespace Project.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        BookStoreContext Context;
        public DiscountRepository(BookStoreContext context)
        {
            Context = context;
        }
        public void Add(Discount discount)
        {
            Context.Add(discount);
        }

        public void DeleteByID(int id)
        {
            Context.Remove(GetById(id));
        }

        public List<Discount> GetAll()
        {
            return Context.Discounts.ToList();
        }

        public Discount GetById(int id)
        {
            return Context.Discounts.SingleOrDefault(d =>d.ID == id);
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void Update(Discount discount)
        {
            Context.Update(discount);
        }
    }
}
