using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.BeerSorts
{
    public class SaveBeerSortCommandHandler : IRequestHandler<SaveBeerSortCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveBeerSortCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveBeerSortCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var beerSort = new BeerSort();
            if (request.Id == 0)
            {
                await _dbContext.BeerSorts.AddAsync(beerSort, cancellationToken);
            }
            else
            {
                beerSort = await _dbContext.BeerSorts.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            beerSort.Name = request.Name;
            beerSort.Description = request.Description;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
