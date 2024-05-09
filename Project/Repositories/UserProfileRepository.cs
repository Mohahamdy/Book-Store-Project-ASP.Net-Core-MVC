using AutoMapper;
using Project.Models;
using Project.ViewModels;
using System.Security.Claims;

namespace Project.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly IMapper mapper;
        private readonly BookStoreContext bookStore;

        public UserProfileRepository(IMapper _mapper, BookStoreContext bookStore)
        {
            mapper = _mapper;
            this.bookStore = bookStore;
        }
        public UserDetails UserDetails(string id)
        {



            UserDetails userInfo = mapper.Map<UserDetails>(bookStore.Users.SingleOrDefault(u => u.Id == int.Parse(id)));
            return userInfo;
        }


        public void Save()
        {
            bookStore.SaveChanges();
        }
    }
}
