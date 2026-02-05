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

            // 1. Fetch the batch AND all related data
            var batch = await _dbContext.BeerBatches
                .Include(b => b.Ingredients)
                .Include(b => b.Logs)
                .Include(b => b.TasteLogs)
                .Include(b => b.Photos)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            // 2. If it's already gone, we are done
            if (batch == null)
            {
                return result;
            }

            // 3. Remove all child relations manually (to satisfy InMemory provider)
            _dbContext.Ingredients.RemoveRange(batch.Ingredients);
            _dbContext.BatchLogs.RemoveRange(batch.Logs);
            _dbContext.TasteLogs.RemoveRange(batch.TasteLogs);
            _dbContext.Photos.RemoveRange(batch.Photos);

            // 4. Remove the batch itself
            _dbContext.BeerBatches.Remove(batch);

            // 5. Save all changes at once
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
