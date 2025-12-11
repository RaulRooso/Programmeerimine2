using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class SaveBeerBatchCommandHandler : IRequestHandler<SaveBeerBatchCommand, OperationResult>
    {
        private readonly IBeerBatchRepository _beerBatchRepository;

        public SaveBeerBatchCommandHandler(IBeerBatchRepository beerBatchRepository)
        {
            _beerBatchRepository = beerBatchRepository;
        }

        public async Task<OperationResult> Handle(SaveBeerBatchCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            BeerBatch batch;

            if (request.Id != 0)
            {
                // Fetch existing batch from repository
                batch = await _beerBatchRepository.GetByIdAsync(request.Id);
            }
            else
            {
                // Create new batch
                batch = new BeerBatch();
            }

            // Map command properties to entity
            batch.Date = request.Date;
            batch.Description = request.Description;
            batch.Conclusion = request.Conclusion;
            batch.BeerSortId = request.BeerSortId;

            // Save using repository
            await _beerBatchRepository.SaveAsync(batch);

            return result;
        }
    }
}
