using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.TasteLogs
{
    public class ListTasteLogsQuery : IRequest<OperationResult<IList<TasteLog>>>
    {
    }
}
