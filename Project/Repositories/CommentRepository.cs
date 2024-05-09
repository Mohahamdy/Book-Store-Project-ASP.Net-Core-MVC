using Project.Models;

namespace Project.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BookStoreContext db;

        public CommentRepository(BookStoreContext db)
        {
            this.db = db;
        }

        public void Add(Comment comment)
        {
            db.Add(comment);
        }

        public void DeleteByBookID(int id)
        {
            db.Remove(GetCommentsByBookId(id));
        }

        public void DeleteByUserID(int id)
        {
            db.Remove(GetCommentsByUserId(id));
        }

        public List<Comment> GetAll()
        {
            return db.Comments.ToList();
        }

        public List<Comment> GetCommentsByBookId(int id)
        {
            return db.Comments.Where(o => o.book_id == id).ToList();

        }
        public List<Comment> GetCommentsByUserId(int id)
        {
            return db.Comments.Where(o => o.user_id == id).ToList();
        }

        public Comment GetCommentByUserAndBookId(int user_id, int book_id)
        {
            return db.Comments.SingleOrDefault(o => o.user_id == user_id && o.book_id == book_id);
        }
        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Comment comment)
        {
            db.Update(comment);
        }
    }
}
