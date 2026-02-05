using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Ingredients
{
    public class GetIngredientQuery : IRequest<OperationResult<IngredientDto>>
    {
        public int Id { get; set; }
    }
}