using AutoMapper;
using EDMN_EgitimPortali_WebAPI.DTOs;
using EDMN_EgitimPortali_WebAPI.Models;
using EDMN_EgitimPortali_WebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDMN_EgitimPortali_WebAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesRepository _repository;
        private readonly SqlServerDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(CategoriesRepository repository, SqlServerDbContext context, IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Educator")]
        public async Task<IActionResult> Create(CreateCategoryDTO dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _repository.AddAsync(category);
            return Ok(_mapper.Map<CategoryDTO>(category));
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetAll()
        {
            var categories = await _repository.GetAllAsync();
            return Ok(_mapper.Map<List<CategoryDTO>>(categories));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(_mapper.Map<CategoryDTO>(category));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Educator")]
        public async Task<IActionResult> Update(int id, CreateCategoryDTO dto)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) return NotFound();

            category.Name = dto.Name;
            await _repository.UpdateAsync(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Educator")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .Include(c => c.CourseCategories)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();

            _context.CourseCategories.RemoveRange(category.CourseCategories);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
