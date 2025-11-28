using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class SaveBeerBatchCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int BeerSortId { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? Conclusion { get; set; }
    }
}
