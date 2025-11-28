using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;

namespace KooliProjekt.Application.Features.TasteLogs
{
    public class SaveTasteLogCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int BeerBatchId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public int Rating { get; set; }
    }
}
