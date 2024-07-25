using AutoMapper;
using SimpleProject.Models;
using SimpleProject.ViewModels.Categories;

namespace SimpleProject.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, GetCategoriesListViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Localize(src.NameAr, src.NameEn)));

            CreateMap<Category, GetCategoryByIdViewModel>();

            CreateMap<AddCategoryViewModel, Category>();

            CreateMap<Category, UpdateCategoryViewModel>();


        }
    }
}
