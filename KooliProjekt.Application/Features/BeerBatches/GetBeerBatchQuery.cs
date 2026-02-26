using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.BeerBatches
{
    [ExcludeFromCodeCoverage]
    public class GetBeerBatchQuery : IRequest<OperationResult<BeerBatchDetailsDto>>
    {
        public int Id { get; set; }
    }
}