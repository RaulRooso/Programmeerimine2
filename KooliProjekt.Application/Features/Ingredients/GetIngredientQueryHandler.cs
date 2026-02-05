using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Ingredients
{
    public class GetIngredientQueryHandler : IRequestHandler<GetIngredientQuery, OperationResult<IngredientDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetIngredientQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<IngredientDto>> Handle(GetIngredientQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<IngredientDto>();
            if (request.Id <= 0) return result;

            result.Value = await _dbContext.Ingredients
                .Where(x => x.Id == request.Id)
                .Select(x => new IngredientDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Unit = x.Unit,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    BeerBatchId = x.BeerBatchId
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}