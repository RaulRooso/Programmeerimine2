using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class DeleteBatchLogCommandHandler : IRequestHandler<DeleteBatchLogCommand, OperationResult>
    {
        private readonly IBatchLogRepository _batchLogRepository;

        public DeleteBatchLogCommandHandler(IBatchLogRepository batchLogRepository)
        {
            _batchLogRepository = batchLogRepository;
        }

        public async Task<OperationResult> Handle(DeleteBatchLogCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            var batchLog = await _batchLogRepository.GetByIdAsync(request.Id);

            if (batchLog != null)
            {
                await _batchLogRepository.DeleteAsync(batchLog);
            }

            return result;
        }
    }
}
