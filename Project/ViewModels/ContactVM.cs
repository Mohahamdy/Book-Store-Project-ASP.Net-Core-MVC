namespace Project.ViewModels
{
    public class ContactVM
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public bool? IsSent { get; set; }
    }
}
