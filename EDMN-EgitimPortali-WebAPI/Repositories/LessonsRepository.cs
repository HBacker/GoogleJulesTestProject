using EDMN_EgitimPortali_WebAPI.Models;
using EDMN_EgitimPortali_WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDMN_EgitimPortali_WebAPI.Repositories
{


    public class LessonsRepository : GenericRepository<Lesson>                                                                                                          , ILessonsRepository
    {
        private readonly SqlServerDbContext _context;

        public LessonsRepository(SqlServerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Lesson>> GetLessonsByCourseIdAsync(int courseId)
        {
            return await _context.Lessons
                .Where(l => l.CourseId == courseId)
                .ToListAsync();
        }
    }
}











public interface ILessonsRepository : IGenericRepository<Lesson>
{
    Task<List<Lesson>> GetLessonsByCourseIdAsync(int courseId);
}