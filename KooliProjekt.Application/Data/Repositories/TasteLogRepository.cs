using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class TasteLogRepository : BaseRepository<TasteLog>, ITasteLogRepository
    {
        public TasteLogRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override async Task<TasteLog> GetByIdAsync(int id)
        {
            return await DbContext
                .TasteLogs
                .Include(t => t.BeerBatch)
                .Include(t => t.User)
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<TasteLog>> ListAsync()
        {
            return await DbContext
                .TasteLogs
                .Include(t => t.BeerBatch)
                .Include(t => t.User)
                .ToListAsync();
        }
    }
}
