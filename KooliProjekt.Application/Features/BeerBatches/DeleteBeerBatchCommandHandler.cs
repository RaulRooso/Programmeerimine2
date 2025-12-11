using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerBatches
{
    public class DeleteBeerBatchCommandHandler : IRequestHandler<DeleteBeerBatchCommand, OperationResult>
    {
        private readonly IBeerBatchRepository _beerBatchRepository;

        public DeleteBeerBatchCommandHandler(IBeerBatchRepository beerBatchRepository)
        {
            _beerBatchRepository = beerBatchRepository;
        }

        public async Task<OperationResult> Handle(DeleteBeerBatchCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // Get the entity first
            var batch = await _beerBatchRepository.GetByIdAsync(request.Id);

            if (batch != null)
            {
                // Delete using repository
                await _beerBatchRepository.DeleteAsync(batch);
            }

            return result;
        }
    }
}
