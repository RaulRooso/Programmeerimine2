using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TasteLogs
{
    public class ListTasteLogsQueryHandler : IRequestHandler<ListTasteLogsQuery, OperationResult<object>>
    {
        private readonly ITasteLogRepository _tasteLogRepository;

        public ListTasteLogsQueryHandler(ITasteLogRepository tasteLogRepository)
        {
            _tasteLogRepository = tasteLogRepository;
        }

        public async Task<OperationResult<object>> Handle(ListTasteLogsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var list = await _tasteLogRepository.ListAsync();
            result.Value = list;

            return result;
        }
    }
}
