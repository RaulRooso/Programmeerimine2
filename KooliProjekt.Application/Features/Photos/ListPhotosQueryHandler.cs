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
    internal class ListPhotosQueryHandler : IRequestHandler<ListPhotosQuery, OperationResult<PagedResult<Photo>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListPhotosQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Photo>>> Handle(ListPhotosQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Photo>>();
            result.Value = await _dbContext
                .Photos
                .OrderBy(p => p.BeerBatchId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
