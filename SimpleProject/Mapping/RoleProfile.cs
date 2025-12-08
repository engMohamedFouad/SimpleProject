using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SimpleProject.Models.Identity;
using SimpleProject.ViewModels.Identity.Roles;

namespace SimpleProject.Mapping
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, GetRolesViewModel>();
            CreateMap<IdentityRole, UpdateRoleViewModel>().ReverseMap();
            CreateMap<IdentityRole, GetRoleByIdViewModel>();
            CreateMap<IdentityRole, DeleteRoleViewModel>();

            CreateMap<User, ManageUsersInRoleViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));
        }
    }
}
