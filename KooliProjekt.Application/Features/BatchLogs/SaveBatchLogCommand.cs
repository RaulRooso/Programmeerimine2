using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.BatchLogs
{
    [ExcludeFromCodeCoverage]
    public class SaveBatchLogCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int BeerBatchId { get; set; }
    }
}
