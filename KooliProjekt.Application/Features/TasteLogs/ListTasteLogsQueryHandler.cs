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

namespace KooliProjekt.Application.Features.TasteLogs
{
    public class ListTasteLogsQueryHandler : IRequestHandler<ListTasteLogsQuery, OperationResult<IList<TasteLog>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListTasteLogsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<TasteLog>>> Handle(ListTasteLogsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<TasteLog>>();
            result.Value = await _dbContext
                .TasteLogs
                .OrderBy(t => t.Date)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}