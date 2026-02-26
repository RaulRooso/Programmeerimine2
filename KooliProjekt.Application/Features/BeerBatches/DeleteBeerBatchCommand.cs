using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.BeerBatches
{
    [ExcludeFromCodeCoverage]
    public class DeleteBeerBatchCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
