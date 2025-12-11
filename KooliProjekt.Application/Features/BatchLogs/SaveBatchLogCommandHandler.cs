using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class SaveBatchLogCommandHandler : IRequestHandler<SaveBatchLogCommand, OperationResult>
    {
        private readonly IBatchLogRepository _batchLogRepository;

        public SaveBatchLogCommandHandler(IBatchLogRepository batchLogRepository)
        {
            _batchLogRepository = batchLogRepository;
        }

        public async Task<OperationResult> Handle(SaveBatchLogCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var batchLog = new BatchLog();
            if (request.Id != 0)
            {
                batchLog = await _batchLogRepository.GetByIdAsync(request.Id);
            }

            batchLog.Date = request.Date;
            batchLog.Description = request.Description;
            batchLog.UserId = request.UserId;
            batchLog.BeerBatchId = request.BeerBatchId;

            await _batchLogRepository.SaveAsync(batchLog);

            return result;
        }
    }
}
