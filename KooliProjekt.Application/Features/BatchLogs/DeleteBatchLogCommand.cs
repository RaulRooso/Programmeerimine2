using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.BatchLogs
{
    [ExcludeFromCodeCoverage]
    public class DeleteBatchLogCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
