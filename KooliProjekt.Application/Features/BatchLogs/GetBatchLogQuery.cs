using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class GetBatchLogQuery : IRequest<OperationResult<BatchLogDto>>
    {
        public int Id { get; set; }
    }
}