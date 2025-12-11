using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class ListBatchLogsQuery : IRequest<OperationResult<object>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
