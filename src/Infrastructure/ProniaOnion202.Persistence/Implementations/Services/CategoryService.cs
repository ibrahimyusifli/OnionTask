using AutoMapper;
using ProniaOnion202.Application.Abstractions.Repositories;
using ProniaOnion202.Application.Abstractions.Services;
using ProniaOnion202.Application.DTOs.Categories;
using ProniaOnion202.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Persistence.Implementations.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }



        public async Task<ICollection<CategoryItemDto>> GetAllAsync(int page, int take)
        {
            ICollection<Category> categories = await _repository.GetAllWhere(skip: (page - 1) * take, take: take).ToListAsync();
            return _mapper.Map < ICollection<CategoryItemDto>>(categories);
        }
       
        public async Task Create(CategoryCreateDto dto)
        {
            await _repository.AddAsync(_mapper.Map<Category>(dto));
            await _repository.SaveChangeAsync();
        }

        public async Task SoftDelete(int id)
        {
            Category category=await _repository.GetByIdAsync(id,true);
            if (category != null) throw new Exception("Not Found");
            _repository.SoftDelete(category);
            await _repository.SaveChangeAsync();
        }
        //public async Task<GetCategoryDto> GetAsync(int id)
        //{
        //    Category category = await _repository.GetByIdAsync(id);
        //    if (category == null) throw new Exception("Not found");
        //    return new GetCategoryDto
        //    {
        //        Id = category.Id,
        //        Name = category.Name,
        //    };
        //}
    }

}
