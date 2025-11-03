using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class ListBatchLogsQuery : IRequest<OperationResult<IList<BatchLog>>>
    {
    }
}
