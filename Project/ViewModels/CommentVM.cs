using Project.Models;

namespace Project.ViewModels
{
    public class CommentVM
    {
		public string comment { get; set; }
        public decimal? rate { get; set; }
        public DateTime Date { get; set; }
        public int user_id { get; set; }
        public int book_id { get; set; }
        public string userFName { get; set; }
        public string userLName { get; set; }
        public bool IsAvailable { get; set; }

    }
}