using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class GetBeerBatchQueryHandler : IRequestHandler<GetBeerBatchQuery, OperationResult<BeerBatchDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetBeerBatchQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<BeerBatchDetailsDto>> Handle(GetBeerBatchQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<BeerBatchDetailsDto>();
            if (request.Id <= 0) return result;

            result.Value = await _dbContext.BeerBatches
                .Include(x => x.Ingredients)
                .Include(x => x.Logs)
                .Include(x => x.Photos)
                .Where(x => x.Id == request.Id)
                .Select(x => new BeerBatchDetailsDto
                {
                    Id = x.Id,
                    Date = x.Date,
                    Description = x.Description,
                    Conclusion = x.Conclusion,
                    BeerSortId = x.BeerSortId,
                    Ingredients = x.Ingredients.Select(i => new IngredientDto
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Unit = i.Unit,
                        Quantity = i.Quantity
                    }).ToList(),
                    Logs = x.Logs.Select(l => new BatchLogDto
                    {
                        Id = l.Id,
                        Date = l.Date,
                        Description = l.Description
                    }).ToList(),
                    Photos = x.Photos.Select(p => new PhotoDto
                    {
                        Id = p.Id,
                        Description = p.Description,
                        FilePath = p.FilePath
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}