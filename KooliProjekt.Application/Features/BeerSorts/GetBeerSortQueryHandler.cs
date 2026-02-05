using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.BeerSorts
{
    public class GetBeerSortQueryHandler : IRequestHandler<GetBeerSortQuery, OperationResult<BeerSortDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetBeerSortQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<BeerSortDto>> Handle(GetBeerSortQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<BeerSortDto>();
            if (request.Id <= 0) return result;

            result.Value = await _dbContext.BeerSorts
                .Where(x => x.Id == request.Id)
                .Select(x => new BeerSortDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}