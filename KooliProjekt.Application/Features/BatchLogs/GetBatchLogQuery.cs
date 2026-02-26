using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.BatchLogs
{
    [ExcludeFromCodeCoverage]
    public class GetBatchLogQuery : IRequest<OperationResult<BatchLogDto>>
    {
        public int Id { get; set; }
    }
}