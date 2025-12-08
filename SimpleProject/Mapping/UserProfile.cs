using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SimpleProject.Helpers;
using SimpleProject.Models.Identity;
using SimpleProject.ViewModels.Identity;
using SimpleProject.ViewModels.Identity.Users;

namespace SimpleProject.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterViewModel, User>()
            .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(des => des.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<User, GetUsersViewModel>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => CultureHelper.IsArabic() ? src.NameAr : src.NameEn));

            CreateMap<User, UpdateUserViewModel>().ReverseMap();

            CreateMap<User, GetUserByIdViewModel>();

            CreateMap<IdentityRole, ManageRolesInUserViewModel>()
                .ForMember(des => des.RoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(des => des.RoleName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
