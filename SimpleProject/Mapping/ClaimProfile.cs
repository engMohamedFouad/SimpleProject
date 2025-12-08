using AutoMapper;
using SimpleProject.Models.Identity;
using SimpleProject.ViewModels.Identity.Claims;

namespace SimpleProject.Mapping
{
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            CreateMap<Claim, GetClaimsViewModel>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Localize(src.NameAr, src.NameEn)));

            CreateMap<AddClaimViewModel, Claim>();
        }
    }
}
