using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.BeerSorts
{
    public class SaveBeerSortCommandHandler : IRequestHandler<SaveBeerSortCommand, OperationResult>
    {
        private readonly IBeerSortRepository _beerSortRepository;

        public SaveBeerSortCommandHandler(IBeerSortRepository beerSortRepository)
        {
            _beerSortRepository = beerSortRepository;
        }

        public async Task<OperationResult> Handle(SaveBeerSortCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var beerSort = new BeerSort();
            if (request.Id != 0)
            {
                beerSort = await _beerSortRepository.GetByIdAsync(request.Id);
            }

            beerSort.Name = request.Name;
            beerSort.Description = request.Description;

            await _beerSortRepository.SaveAsync(beerSort);

            return result;
        }
    }
}
