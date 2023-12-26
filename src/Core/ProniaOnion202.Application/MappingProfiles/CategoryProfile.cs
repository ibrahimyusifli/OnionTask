using AutoMapper;
using ProniaOnion202.Application.DTOs.Categories;
using ProniaOnion202.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Application.MappingProfiles
{
    internal class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryItemDto>().ReverseMap();
            CreateMap<Category, IncludeCategoryDto>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>();

        }
    }
}
