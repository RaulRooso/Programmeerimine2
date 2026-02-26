using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.TasteLogs
{
    [ExcludeFromCodeCoverage]
    public class ListTasteLogsQuery : IRequest<OperationResult<PagedResult<TasteLog>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Description { get; set; }
    }
}
