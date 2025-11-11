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

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class ListBatchLogsQueryHandler : IRequestHandler<ListBatchLogsQuery, OperationResult<PagedResult<BatchLog>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListBatchLogsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<BatchLog>>> Handle(ListBatchLogsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<BatchLog>>();
            result.Value = await _dbContext
                .BatchLogs
                .OrderBy(l => l.Date)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
