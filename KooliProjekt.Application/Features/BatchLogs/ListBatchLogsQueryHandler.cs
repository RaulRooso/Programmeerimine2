using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class ListBatchLogsQueryHandler : IRequestHandler<ListBatchLogsQuery, OperationResult<PagedResult<BatchLog>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListBatchLogsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PagedResult<BatchLog>>> Handle(ListBatchLogsQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<PagedResult<BatchLog>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            var query = _dbContext.BatchLogs.AsQueryable();

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
