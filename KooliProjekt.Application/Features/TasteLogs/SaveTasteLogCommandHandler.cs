using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TasteLogs
{
    public class SaveTasteLogCommandHandler : IRequestHandler<SaveTasteLogCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveTasteLogCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveTasteLogCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var tasteLog = new TasteLog();
            if (request.Id == 0)
            {
                await _dbContext.TasteLogs.AddAsync(tasteLog, cancellationToken);
            }
            else
            {
                tasteLog = await _dbContext.TasteLogs.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            tasteLog.BeerBatchId = request.BeerBatchId;
            tasteLog.UserId = request.UserId;
            tasteLog.Date = request.Date;
            tasteLog.Description = request.Description;
            tasteLog.Rating = request.Rating;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
