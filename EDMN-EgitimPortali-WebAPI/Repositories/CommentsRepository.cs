using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EDMN_EgitimPortali_WebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using EDMN_EgitimPortali_WebAPI.Repositories;

namespace EDMN_EgitimPortali_WebAPI.Repositories
{


    public class CommentsRepository : GenericRepository<Comment>                                                                                                                , ICommentsRepository
    {
        private readonly SqlServerDbContext _context;

        public CommentsRepository(SqlServerDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Comment> GetCommentWithUser(int id)
        {
            return await _context.Comments.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Comment>> GetCommentsByCourseId(int courseId) 
        {
            return await _context.Comments
                .Where(c => c.CourseId == courseId)
                .Include(c => c.User) 
                .ToListAsync();
        }
    }
}




























public interface ICommentsRepository : IGenericRepository<Comment>
{
    Task<Comment> GetCommentWithUser(int id);
    Task<List<Comment>> GetCommentsByCourseId(int courseId);
}