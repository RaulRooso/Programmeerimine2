using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class ListBeerBatchesQueryHandler : IRequestHandler<ListBeerBatchesQuery, OperationResult<PagedResult<BeerBatch>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListBeerBatchesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<BeerBatch>>> Handle(ListBeerBatchesQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<PagedResult<BeerBatch>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            var query = _dbContext.BeerBatches.AsQueryable();

            if (!string.IsNullOrEmpty(request.Description))
            {
                query = query.Where(x => x.Description.Contains(request.Description));
            }

            result.Value = await query
                .OrderByDescending(x => x.Date)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
