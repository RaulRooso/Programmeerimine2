using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Ingredients
{
    [ExcludeFromCodeCoverage]
    public class DeleteIngredientCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
