using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Ingredients
{
    public class ListIngredientsQueryHandler : IRequestHandler<ListIngredientsQuery, OperationResult<object>>
    {
        private readonly IIngredientRepository _ingredientRepository;

        public ListIngredientsQueryHandler(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<OperationResult<object>> Handle(ListIngredientsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var list = await _ingredientRepository.ListAsync();
            result.Value = list;

            return result;
        }
    }
}
