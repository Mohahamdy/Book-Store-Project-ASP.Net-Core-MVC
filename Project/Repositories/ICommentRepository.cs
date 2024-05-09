using Project.Models;

namespace Project.Repositories
{
    public interface ICommentRepository
    {
        public List<Comment> GetAll();



        public void Add(Comment comment);
        public void Update(Comment comment);
        public void DeleteByBookID(int id);
        public void DeleteByUserID(int id);
        public List<Comment> GetCommentsByBookId(int id);
        public List<Comment> GetCommentsByUserId(int id);
        public Comment GetCommentByUserAndBookId(int user_id, int book_id);

        public void Save();
    }
}