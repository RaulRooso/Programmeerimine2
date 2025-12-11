using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IBatchLogRepository
    {
        Task<BatchLog> GetByIdAsync(int id);
        Task SaveAsync(BatchLog batchLog);
        Task DeleteAsync(BatchLog batchLog);

        Task<List<BatchLog>> ListAsync();
    }
}
