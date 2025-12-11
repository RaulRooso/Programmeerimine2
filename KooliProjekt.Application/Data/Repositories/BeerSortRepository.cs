using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class BeerSortRepository : BaseRepository<BeerSort>, IBeerSortRepository
    {
        public BeerSortRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override async Task<BeerSort> GetByIdAsync(int id)
        {
            return await DbContext
                .BeerSorts
                .Include(bs => bs.Batches)
                .Where(bs => bs.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<BeerSort>> ListAsync()
        {
            return await DbContext
                .BeerSorts
                .Include(bs => bs.Batches)
                .ToListAsync();
        }
    }
}
