using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class SaveBatchLogCommandHandler : IRequestHandler<SaveBatchLogCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveBatchLogCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveBatchLogCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var log = new BatchLog();
            if (request.Id == 0)
            {
                await _dbContext.BatchLogs.AddAsync(log, cancellationToken);
            }
            else
            {
                log = await _dbContext.BatchLogs.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            log.Date = request.Date;
            log.Description = request.Description;
            log.UserId = request.UserId;
            log.BeerBatchId = request.BeerBatchId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
