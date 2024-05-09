using AutoMapper;
using Project.Models;
using Project.ViewModels;

namespace Project.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<ApplicationUser, UserDetails>();
        }
    }
}
