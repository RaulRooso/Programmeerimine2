using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IBeerSortRepository
    {
        Task<BeerSort> GetByIdAsync(int id);
        Task SaveAsync(BeerSort entity);
        Task DeleteAsync(BeerSort entity);
        Task<IList<BeerSort>> ListAsync();
    }
}
