using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerSorts
{
    public class GetBeerSortQuery : IRequest<OperationResult<BeerSortDto>>
    {
        public int Id { get; set; }
    }
}