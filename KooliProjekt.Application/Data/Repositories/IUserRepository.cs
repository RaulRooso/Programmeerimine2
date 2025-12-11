using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task SaveAsync(User entity);
        Task DeleteAsync(User entity);
        Task<IList<User>> ListAsync();
    }
}
