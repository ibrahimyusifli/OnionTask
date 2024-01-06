using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProniaOnion202.Application.Abstractions.Services;
using ProniaOnion202.Application.DTOs.Products;
using System.Security.Claims;

namespace ProniaOnion202.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll(int page,int take)
        {
            return Ok (await _service.GetAllAsync(page, take));
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get(int id)
        {
           
            if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest);
            return StatusCode (StatusCodes.Status200OK,await _service.GetByIdAsync(id));
           
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]ProductCreateDto dto)
        {
            await _service.CreateAsync(dto);
            return StatusCode(StatusCodes.Status201Created); 
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,ProductUpdateDto )
        {
            if (id <= 0) return StatusCode(StatusCodes.Status400BadRequest);
            await _service.UpdateAsync(id,dto);
            return StatusCode(StatusCodes.Status204NoContent);
        
        }
    }
}
