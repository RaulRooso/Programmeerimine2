using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class ListBatchLogsQueryHandler : IRequestHandler<ListBatchLogsQuery, OperationResult<object>>
    {
        private readonly IBatchLogRepository _batchLogRepository;

        public ListBatchLogsQueryHandler(IBatchLogRepository batchLogRepository)
        {
            _batchLogRepository = batchLogRepository;
        }

        public async Task<OperationResult<object>> Handle(ListBatchLogsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var batchLogs = await _batchLogRepository.ListAsync();

            result.Value = batchLogs.Select(b => new
            {
                b.Id,
                b.Date,
                b.Description,
                User = new { b.User.Id, b.User.Username },
                BeerBatch = new { b.BeerBatch.Id, b.BeerBatch.Description }
            });

            return result;
        }
    }
}
