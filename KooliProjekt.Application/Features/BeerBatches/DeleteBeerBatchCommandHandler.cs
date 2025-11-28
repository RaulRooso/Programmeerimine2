using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class DeleteBeerBatchCommandHandler : IRequestHandler<DeleteBeerBatchCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteBeerBatchCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteBeerBatchCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // kdelete relations:
            await _dbContext.Ingredients
                .Where(i => i.BeerBatchId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await _dbContext.BatchLogs
                .Where(l => l.BeerBatchId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await _dbContext.TasteLogs
                .Where(t => t.BeerBatchId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            await _dbContext.Photos
                .Where(p => p.BeerBatchId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // delete batches
            await _dbContext.BeerBatches
                .Where(b => b.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
