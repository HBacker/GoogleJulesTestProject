using EDMN_EgitimPortali_WebAPI.DTOs;
using EDMN_EgitimPortali_WebAPI.Models;
using EDMN_EgitimPortali_WebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;

namespace EDMN_EgitimPortali_WebAPI.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IMapper _mapper;

        public CoursesController(ICoursesRepository coursesRepository, IMapper mapper)
        {
            _coursesRepository = coursesRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _coursesRepository.GetAllAsync();

            if (courses == null || !courses.Any())
            {
                return NotFound("Kayıt yok.");
            }

            var courseDtos = _mapper.Map<IEnumerable<CourseDTO>>(courses);

            return Ok(courseDtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _coursesRepository.GetByIdAsync(id); 
            if (course == null)
                return NotFound();

            var courseDto = _mapper.Map<CourseDTO>(course);
            return Ok(courseDto);
        }



        [HttpPost]
        [Authorize(Roles = "Educator,Admin")]
        public async Task<IActionResult> AddCourse([FromBody] CourseCreateDTO courseCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = _mapper.Map<Course>(courseCreateDto);
            course.EducatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var categoryIds = courseCreateDto.CategoryIds;

            var result = await _coursesRepository.AddCourseWithCategoriesAsync(course, categoryIds);

            if (!result)
            {
                return BadRequest("Geçerli kategoriler bulunamadı.");
            }

            var courseDto = _mapper.Map<CourseDTO>(course);

            var categories = await _coursesRepository.GetCourseCategoriesAsync(course.Id);

            if (categories != null)
            {
                courseDto.CategoryIds = categories.Select(c => c.Category.Id).ToList();
            }

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, courseDto);
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseUpdateDTO courseUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var existingCourse = await _coursesRepository.GetByIdAsync(id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            if (existingCourse.EducatorId == userId || userRoles.Contains("Admin"))
            {
                var categoryIds = courseUpdateDto.CategoryIds;  

                var result = await _coursesRepository.UpdateCourseWithCategoriesAsync(id, courseUpdateDto, categoryIds);

                if (!result)
                {
                    return BadRequest("Geçerli kategoriler bulunamadı.");
                }

                return NoContent();
            }

            return Forbid();
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var existingCourse = await _coursesRepository.GetByIdAsync(id);
            if (existingCourse == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            if (existingCourse.EducatorId == userId || userRoles.Contains("Admin"))
            {
                await _coursesRepository.DeleteAsync(id);
                return NoContent();
            }

            return Forbid();
        }
    }
}
