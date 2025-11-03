using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class ListBeerBatchesQueryHandler : IRequestHandler<ListBeerBatchesQuery, OperationResult<IList<BeerBatch>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListBeerBatchesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<OperationResult<IList<BeerBatch>>> Handle(ListBeerBatchesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<BeerBatch>>();
            result.Value = await _dbContext
                .BeerBatches
                .OrderBy(b => b.Date)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
