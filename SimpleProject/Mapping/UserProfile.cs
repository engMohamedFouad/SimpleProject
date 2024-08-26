using AutoMapper;
using SimpleProject.Models.Identity;
using SimpleProject.ViewModels.Identity;

namespace SimpleProject.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterViewModel, User>()
            .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(des => des.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
