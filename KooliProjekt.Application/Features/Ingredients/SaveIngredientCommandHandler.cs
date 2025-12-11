using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Ingredients
{
    public class SaveIngredientCommandHandler : IRequestHandler<SaveIngredientCommand, OperationResult>
    {
        private readonly IIngredientRepository _ingredientRepository;

        public SaveIngredientCommandHandler(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<OperationResult> Handle(SaveIngredientCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var ingredient = new Ingredient();
            if (request.Id != 0)
            {
                ingredient = await _ingredientRepository.GetByIdAsync(request.Id);
            }

            ingredient.Name = request.Name;
            ingredient.Unit = request.Unit;
            ingredient.UnitPrice = request.UnitPrice;
            ingredient.Quantity = request.Quantity;
            ingredient.BeerBatchId = request.BeerBatchId;

            await _ingredientRepository.SaveAsync(ingredient);

            return result;
        }
    }
}
