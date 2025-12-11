using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data.Repositories
{
    public class IngredientRepository : BaseRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public override async Task<Ingredient> GetByIdAsync(int id)
        {
            return await DbContext
                .Ingredients
                .Include(i => i.BeerBatch)
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<Ingredient>> ListAsync()
        {
            return await DbContext
                .Ingredients
                .Include(i => i.BeerBatch)
                .ToListAsync();
        }
    }
}
