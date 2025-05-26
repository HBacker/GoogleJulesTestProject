using EDMN_EgitimPortali_WebAPI.Models;

namespace EDMN_EgitimPortali_WebAPI.Repositories
{
    public class CategoriesRepository : GenericRepository<Category>
    {
        public CategoriesRepository(SqlServerDbContext context) : base(context)
        {
        }
    }
}
