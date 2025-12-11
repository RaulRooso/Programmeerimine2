using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class ListBeerBatchesQueryHandler : IRequestHandler<ListBeerBatchesQuery, OperationResult<object>>
    {
        private readonly IBeerBatchRepository _beerBatchRepository;

        public ListBeerBatchesQueryHandler(IBeerBatchRepository beerBatchRepository)
        {
            _beerBatchRepository = beerBatchRepository;
        }

        public async Task<OperationResult<object>> Handle(ListBeerBatchesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var batches = await _beerBatchRepository.ListAsync();

            // Project to anonymous objects for API response
            result.Value = batches.Select(b => new
            {
                b.Id,
                b.Date,
                b.Description,
                b.Conclusion,
                BeerSort = new
                {
                    b.BeerSort.Id,
                    b.BeerSort.Name
                },
                IngredientCount = b.Ingredients.Count,
                LogCount = b.Logs.Count,
                TasteLogCount = b.TasteLogs.Count,
                PhotoCount = b.Photos.Count
            });

            return result;
        }
    }
}
