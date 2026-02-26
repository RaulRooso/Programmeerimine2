using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Ingredients
{
    [ExcludeFromCodeCoverage]
    public class GetIngredientQuery : IRequest<OperationResult<IngredientDto>>
    {
        public int Id { get; set; }
    }
}