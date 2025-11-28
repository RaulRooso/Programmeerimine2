using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class DeleteBeerBatchCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
