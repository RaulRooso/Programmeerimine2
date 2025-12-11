using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override async Task<Photo> GetByIdAsync(int id)
        {
            return await DbContext
                .Photos
                .Include(p => p.BeerBatch)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<Photo>> ListAsync()
        {
            return await DbContext
                .Photos
                .Include(p => p.BeerBatch)
                .ToListAsync();
        }
    }
}
