using AutoMapper;
using SimpleProject.Models;
using SimpleProject.ViewModels;

namespace SimpleProject.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductViewModel, Product>();
        }
    }
}
