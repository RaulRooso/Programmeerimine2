using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerSorts
{
    public class ListBeerSortsQueryHandler : IRequestHandler<ListBeerSortsQuery, OperationResult<object>>
    {
        private readonly IBeerSortRepository _beerSortRepository;

        public ListBeerSortsQueryHandler(IBeerSortRepository beerSortRepository)
        {
            _beerSortRepository = beerSortRepository;
        }

        public async Task<OperationResult<object>> Handle(ListBeerSortsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var list = await _beerSortRepository.ListAsync();

            result.Value = list;

            return result;
        }
    }
}
