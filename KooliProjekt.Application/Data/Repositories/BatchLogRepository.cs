using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class BatchLogRepository : BaseRepository<BatchLog>, IBatchLogRepository
    {
        public BatchLogRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<BatchLog> GetByIdAsync(int id)
        {
            return await DbContext.BatchLogs
                .Include(b => b.User)
                .Include(b => b.BeerBatch)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<BatchLog>> ListAsync()
        {
            return await DbContext.BatchLogs
                .Include(b => b.User)
                .Include(b => b.BeerBatch)
                .ToListAsync();
        }
    }
}
