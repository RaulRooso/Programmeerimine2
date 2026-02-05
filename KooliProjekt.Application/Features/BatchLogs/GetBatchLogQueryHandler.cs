using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class GetBatchLogQueryHandler : IRequestHandler<GetBatchLogQuery, OperationResult<BatchLogDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetBatchLogQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<BatchLogDto>> Handle(GetBatchLogQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<BatchLogDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext.BatchLogs
                .Where(x => x.Id == request.Id)
                .Select(x => new BatchLogDto
                {
                    Id = x.Id,
                    Date = x.Date, // Corrected: was LogDate
                    Description = x.Description,
                    UserId = x.UserId,
                    BeerBatchId = x.BeerBatchId
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}