using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IPhotoRepository
    {
        Task<Photo> GetByIdAsync(int id);
        Task SaveAsync(Photo entity);
        Task DeleteAsync(Photo entity);
        Task<IList<Photo>> ListAsync();
    }
}
