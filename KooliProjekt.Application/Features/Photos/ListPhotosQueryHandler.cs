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

namespace KooliProjekt.Application.Features.Photos
{
    internal class ListPhotosQueryHandler : IRequestHandler<ListPhotosQuery, OperationResult<IList<Photo>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListPhotosQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Photo>>> Handle(ListPhotosQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Photo>>();
            result.Value = await _dbContext
                .Photos
                .OrderBy(p => p.BeerBatchId)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
