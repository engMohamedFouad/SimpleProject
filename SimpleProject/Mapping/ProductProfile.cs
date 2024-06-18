﻿using AutoMapper;
using SimpleProject.Models;
using SimpleProject.ViewModels;

namespace SimpleProject.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductViewModel, Product>();

            CreateMap<Product, GetProductListViewModel>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Localize(src.NameAr, src.NameEn)));
        }
    }
}
