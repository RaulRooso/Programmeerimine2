using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerSorts
{
    public class DeleteBeerSortCommandHandler : IRequestHandler<DeleteBeerSortCommand, OperationResult>
    {
        private readonly IBeerSortRepository _beerSortRepository;

        public DeleteBeerSortCommandHandler(IBeerSortRepository beerSortRepository)
        {
            _beerSortRepository = beerSortRepository;
        }

        public async Task<OperationResult> Handle(DeleteBeerSortCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var beerSort = await _beerSortRepository.GetByIdAsync(request.Id);

            if (beerSort != null)
            {
                await _beerSortRepository.DeleteAsync(beerSort);
            }

            return result;
        }
    }
}
