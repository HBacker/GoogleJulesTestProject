using EDMN_EgitimPortali_WebAPI.DTOs;
using EDMN_EgitimPortali_WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using EDMN_EgitimPortali_WebAPI.Repositories;

namespace EDMN_EgitimPortali_WebAPI.Repositories
{


    public class CoursesRepository : GenericRepository<Course>                                                                                                         ,ICoursesRepository
    {
        private readonly SqlServerDbContext _context;

        public CoursesRepository(SqlServerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Course> GetCourseWithLessonsAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Lessons)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> AddCourseWithCategoriesAsync(Course course, List<int> categoryIds)
        {
            try
            {
                foreach (var categoryId in categoryIds)
                {
                    if (!course.CourseCategories.Any(cc => cc.CategoryId == categoryId))
                    {
                        var courseCategory = new CourseCategory
                        {
                            CourseId = course.Id,
                            CategoryId = categoryId
                        };
                        course.CourseCategories.Add(courseCategory);
                    }
                }

                await _context.Courses.AddAsync(course);
                await _context.SaveChangesAsync();

                return true;  
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCourseWithCategoriesAsync(int courseId, CourseUpdateDTO courseUpdateDto, List<int> categoryIds)
        {
            var course = await _context.Courses
                                       .Include(c => c.CourseCategories)
                                       .ThenInclude(cc => cc.Category)
                                       .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return false;  
            }

            var categories = await _context.Categories
                                           .Where(c => categoryIds.Contains(c.Id))
                                           .ToListAsync();

            if (categories.Count != categoryIds.Count)
            {
                return false;
            }

            course.CourseCategories.Clear();
            foreach (var category in categories)
            {
                course.CourseCategories.Add(new CourseCategory { Course = course, Category = category });
            }

            course.Title = courseUpdateDto.Title;
            course.Description = courseUpdateDto.Description;
            course.PhotoUrl = courseUpdateDto.PhotoUrl;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CourseCategory>> GetCourseCategoriesAsync(int courseId)
        {
            return await _context.CourseCategories
                                 .Include(cc => cc.Category)
                                 .Where(cc => cc.CourseId == courseId)
                                 .ToListAsync();
        }
        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            var courses = await _context.Courses
                .Include(course => course.CourseCategories)
                .ThenInclude(courseCategory => courseCategory.Category)
                .ToListAsync();

            foreach (var course in courses)
            {
                Console.WriteLine($"Course ID: {course.Id}, Category Count: {course.CourseCategories.Count}");
            }

            return courses;
        }


        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses
                .Include(course => course.CourseCategories)  
                .ThenInclude(courseCategory => courseCategory.Category) 
                .FirstOrDefaultAsync(course => course.Id == id);
        }

    }
}



















public interface ICoursesRepository : IGenericRepository<Course>
{
    Task<Course> GetCourseWithLessonsAsync(int id);
    Task<bool> AddCourseWithCategoriesAsync(Course course, List<int> categoryIds);
    Task<bool> UpdateCourseWithCategoriesAsync(int courseId, CourseUpdateDTO courseUpdateDto, List<int> categoryIds);
    Task<List<CourseCategory>> GetCourseCategoriesAsync(int courseId);
    Task<IEnumerable<Course>> GetAllAsync();
    Task<Course> GetByIdAsync(int id);

}