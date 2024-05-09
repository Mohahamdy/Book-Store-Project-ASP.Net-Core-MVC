using Project.ViewModels;

namespace Project.Repositories
{
    public interface IUserProfileRepository
    {

        public UserDetails UserDetails(string id);
        public void Save();


    }
}