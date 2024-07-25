using AutoMapper;
using SimpleProject.Models;
using SimpleProject.ViewModels.Products;

namespace SimpleProject.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductViewModel, Product>();

            CreateMap<Product, GetProductListViewModel>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Localize(src.NameAr, src.NameEn)));

            CreateMap<Product, UpdateProductViewModel>()
                 .ForMember(des => des.CurrentPaths, opt => opt.MapFrom(src => src.ProductsImages.Select(x => x.Path).ToList()));

            CreateMap<UpdateProductViewModel, Product>();
        }
    }
}
