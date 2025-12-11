using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IBeerBatchRepository
    {
        Task<BeerBatch> GetByIdAsync(int id);
        Task SaveAsync(BeerBatch batch);
        Task DeleteAsync(BeerBatch batch);

        // Add this:
        Task<List<BeerBatch>> ListAsync();
    }
}
