using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Ingredients
{
    public class DeleteIngredientCommandHandler : IRequestHandler<DeleteIngredientCommand, OperationResult>
    {
        private readonly IIngredientRepository _ingredientRepository;

        public DeleteIngredientCommandHandler(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<OperationResult> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var ingredient = await _ingredientRepository.GetByIdAsync(request.Id);

            if (ingredient != null)
            {
                await _ingredientRepository.DeleteAsync(ingredient);
            }

            return result;
        }
    }
}
