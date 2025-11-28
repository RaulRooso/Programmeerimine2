using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerSorts
{
    public class DeleteBeerSortCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
