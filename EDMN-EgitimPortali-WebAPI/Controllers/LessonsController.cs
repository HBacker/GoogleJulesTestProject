using EDMN_EgitimPortali_WebAPI.Models;
using EDMN_EgitimPortali_WebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using AutoMapper;
using EDMN_EgitimPortali_WebAPI.DTOs.Lessons; 
using System.Collections.Generic;

namespace EDMN_EgitimPortali_WebAPI.Controllers
{
    [ApiController]
    [Route("api/lessons")]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonsRepository _lessonsRepository;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IMapper _mapper;

        public LessonsController(ILessonsRepository lessonsRepository, ICoursesRepository coursesRepository, IMapper mapper)
        {
            _lessonsRepository = lessonsRepository;
            _coursesRepository = coursesRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Educator,Admin")]
        public async Task<IActionResult> AddLesson([FromBody] LessonCreateDTO lessonCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _coursesRepository.GetByIdAsync(lessonCreateDto.CourseId);
            if (course == null)
            {
                return NotFound("Kurs bulunumadı.");
            }

            var educatorId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (course.EducatorId != educatorId && !User.IsInRole("Admin"))
            {
                return Forbid("Bu kursa ders eklemek için yetkiniz yok.");
            }

            var lesson = _mapper.Map<Lesson>(lessonCreateDto);
            lesson.EducatorId = educatorId;
            await _lessonsRepository.AddAsync(lesson);
            var lessonDto = _mapper.Map<LessonDTO>(lesson);
            return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lessonDto);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLesson(int id)
        {
            var lesson = await _lessonsRepository.GetByIdAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }
            var lessonDto = _mapper.Map<LessonDTO>(lesson); 
            return Ok(lessonDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Educator,Admin")]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] LessonDTO lessonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingLesson = await _lessonsRepository.GetByIdAsync(id);
            if (existingLesson == null)
            {
                return NotFound();
            }

            var course = await _coursesRepository.GetByIdAsync(existingLesson.CourseId);
            if (course == null)
            {
                return NotFound("Kurs bulunumadı.");
            }

            if (course.EducatorId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            {
                return Forbid("Bu dersi güncellemek için yetkiniz yok.");
            }
            existingLesson.Title = lessonDto.Title;
            existingLesson.VideoUrl = lessonDto.VideoUrl;
            existingLesson.ThumbnailUrl = lessonDto.ThumbnailUrl;
            existingLesson.OrderNo = lessonDto.OrderNo;
            existingLesson.EducatorId = lessonDto.EducatorId;
            existingLesson.CourseId = lessonDto.CourseId;
            await _lessonsRepository.UpdateAsync(existingLesson);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Educator,Admin")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _lessonsRepository.GetByIdAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }
            var course = await _coursesRepository.GetByIdAsync(lesson.CourseId);
            if (course == null)
            {
                return NotFound("Course not found");
            }
            if (course.EducatorId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            {
                return Forbid("Bu dersi silmek için yetkiniz yok.");
            }

            await _lessonsRepository.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLessons()
        {
            var lessons = await _lessonsRepository.GetAllAsync();
            var lessonDtos = _mapper.Map<List<LessonDTO>>(lessons);
            return Ok(lessonDtos);
        }
    }
}
