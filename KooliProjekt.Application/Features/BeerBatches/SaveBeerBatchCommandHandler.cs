using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class SaveBeerBatchCommandHandler : IRequestHandler<SaveBeerBatchCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveBeerBatchCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveBeerBatchCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var batch = new BeerBatch();
            if (request.Id == 0)
            {
                await _dbContext.BeerBatches.AddAsync(batch, cancellationToken);
            }
            else
            {
                batch = await _dbContext.BeerBatches.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            batch.BeerSortId = request.BeerSortId;
            batch.Date = request.Date;
            batch.Description = request.Description;
            batch.Conclusion = request.Conclusion;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
