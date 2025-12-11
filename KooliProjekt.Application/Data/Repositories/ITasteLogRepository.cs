using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface ITasteLogRepository
    {
        Task<TasteLog> GetByIdAsync(int id);
        Task SaveAsync(TasteLog entity);
        Task DeleteAsync(TasteLog entity);
        Task<IList<TasteLog>> ListAsync();
    }
}
