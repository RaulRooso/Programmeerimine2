using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;


namespace KooliProjekt.Application.Features.BeerBatches
{
    public class ListBeerBatchesQuery : IRequest<OperationResult<IList<BeerBatch>>>
    {
    }
}
