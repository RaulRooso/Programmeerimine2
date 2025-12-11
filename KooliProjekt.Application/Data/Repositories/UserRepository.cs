using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await DbContext
                .Users
                .Include(u => u.BatchLogs)
                .Include(u => u.TasteLogs)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<User>> ListAsync()
        {
            return await DbContext
                .Users
                .Include(u => u.BatchLogs)
                .Include(u => u.TasteLogs)
                .ToListAsync();
        }
    }
}
