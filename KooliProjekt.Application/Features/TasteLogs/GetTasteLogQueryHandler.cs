using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.TasteLogs
{
    public class GetTasteLogQueryHandler : IRequestHandler<GetTasteLogQuery, OperationResult<TasteLogDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetTasteLogQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<TasteLogDto>> Handle(GetTasteLogQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<TasteLogDto>();
            if (request.Id <= 0) return result;

            result.Value = await _dbContext.TasteLogs
                .Where(x => x.Id == request.Id)
                .Select(x => new TasteLogDto
                {
                    Id = x.Id,
                    Date = x.Date,
                    Description = x.Description,
                    Rating = x.Rating,
                    UserId = x.UserId,
                    BeerBatchId = x.BeerBatchId
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}