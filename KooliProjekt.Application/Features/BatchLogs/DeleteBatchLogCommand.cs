using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class DeleteBatchLogCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
