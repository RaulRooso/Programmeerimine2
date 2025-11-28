using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Ingredients
{
    public class DeleteIngredientCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
