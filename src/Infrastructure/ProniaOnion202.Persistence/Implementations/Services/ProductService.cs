using AutoMapper;
using ProniaOnion202.Application.Abstractions.Repositories;
using ProniaOnion202.Application.Abstractions.Services;
using ProniaOnion202.Application.DTOs.Products;
using ProniaOnion202.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Persistence.Implementations.Services
{
    internal class ProductService:IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository,ICategoryRepository categoryRepository,IColorRepository colorRepository,IMapper mapper)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _colorRepository = colorRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProductItemDto>> GetAllAsync(int page,int take)
        {
            return _mapper.Map<IEnumerable<ProductItemDto>>(await _repository.GetAllWhere(skip: (page - 1) * take, take: take).ToListAsync());
        }
        public async Task<ProductGetDto> GetByIdAsync(int id)
        {
            Product product = await _repository.GetByIdAsync(id,includes:nameof(Product.Category));
            if (product == null) throw new Exception("Bele bir mehsul yoxdur");
            return _mapper.Map<ProductGetDto>(product);
        }
        public async Task CreateAsync(ProductCreateDto dto)
        {
            if (await _repository.IsExistAsync(p => p.Name == dto.Name)) throw new Exception("Same");
)
            if (!await _categoryRepository.IsExistAsync(c => c.Id == dto.CategoryId))throw new Exception("Dont");

            Product product=_mapper.Map<Product>(dto);
            product.ProductColors = new List<ProductColor>();


            if (dto.ColorIds is not null)
            {
               foreach (var colId in dto.ColorIds)
                 {
                    if (!await _colorRepository.IsExistAsync(c => c.Id == colId)) throw new Exception("Dont");
                     product.ProductColors.Add(new ProductColor { ColorId= colId });

                 }       
            }
            await _repository.AddAsync(product);
            await _repository.SaveChangeAsync();
        }
        public async Task UpdateAsync(int id,ProductUpdateDto dto)
        {
            Product existed = await _repository.GetByIdAsync(id,true,includes:nameof(Product.ProductColors));
            if (existed.Name != dto.Name)
                if (await _repository.IsExistAsync(p => p.Name == dto.Name)) throw new Exception("Same");

            if (dto.CategoryId!=existed.CategoryId)
                if (!await _categoryRepository.IsExistAsync(c => c.Id == dto.CategoryId)) throw new Exception("Dont");

            existed = _mapper.Map(dto, existed);
            if (dto.ColorIds is not null)
            {
                foreach (var colorId in dto.ColorIds)
                {
                    if (!existed.ProductColors.Any(pc => pc.ColorId == colorId))
                    {
                        if (!await _colorRepository.IsExistAsync(c => c.Id == colorId)) throw new Exception("Dont");
                        existed.ProductColors.Add(new ProductColor { ColorId = colorId });
                    }
                }
                existed.ProductColors = existed.ProductColors.Where(pc => dto.ColorIds.Any(colId => pc.ColorId == colId)).ToList();
            }
            else
                existed.ProductColors=new List<ProductColor>();

           
  
            _repository.Update(existed);
            await _repository.SaveChangeAsync();
        }
    }
}
