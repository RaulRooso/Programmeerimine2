using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class ListBeerBatchesQuery : IRequest<OperationResult<object>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
