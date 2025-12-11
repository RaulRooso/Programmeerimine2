using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IIngredientRepository
    {
        Task<Ingredient> GetByIdAsync(int id);
        Task SaveAsync(Ingredient entity);
        Task DeleteAsync(Ingredient entity);
        Task<IList<Ingredient>> ListAsync();
    }
}
