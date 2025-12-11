using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class BeerBatchRepository : BaseRepository<BeerBatch>, IBeerBatchRepository
    {
        public BeerBatchRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<BeerBatch> GetByIdAsync(int id)
        {
            return await DbContext.BeerBatches
                .Include(b => b.BeerSort)
                .Include(b => b.Ingredients)
                .Include(b => b.Logs)
                .Include(b => b.TasteLogs)
                .Include(b => b.Photos)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<BeerBatch>> ListAsync()
        {
            return await DbContext.BeerBatches
                .Include(b => b.BeerSort)
                .Include(b => b.Ingredients)
                .Include(b => b.Logs)
                .Include(b => b.TasteLogs)
                .Include(b => b.Photos)
                .ToListAsync();
        }
    }
}
