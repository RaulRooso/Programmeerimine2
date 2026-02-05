using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class GetBeerBatchQuery : IRequest<OperationResult<BeerBatchDetailsDto>>
    {
        public int Id { get; set; }
    }
}