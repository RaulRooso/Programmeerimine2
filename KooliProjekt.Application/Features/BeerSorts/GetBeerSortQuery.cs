using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.BeerSorts
{
    [ExcludeFromCodeCoverage]
    public class GetBeerSortQuery : IRequest<OperationResult<BeerSortDto>>
    {
        public int Id { get; set; }
    }
}