using EDMN_EgitimPortali_WebAPI.Models;
using EDMN_EgitimPortali_WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDMN_EgitimPortali_WebAPI.Repositories
{

    public class WatchedContentsRepository : GenericRepository<WatchedContent>                                                                                              , IWatchedContentsRepository
    {
        private readonly SqlServerDbContext _context;

        public WatchedContentsRepository(SqlServerDbContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<WatchedContent> GetWatchedContentAsync(string userId, int lessonId)
        {
            return await _context.WatchedContents
                .FirstOrDefaultAsync(x => x.UserId == userId && x.LessonId == lessonId);
        }
       
    }
   
}





public interface IWatchedContentsRepository : IGenericRepository<WatchedContent>
{
    Task<WatchedContent> GetWatchedContentAsync(string userId, int lessonId);
}
