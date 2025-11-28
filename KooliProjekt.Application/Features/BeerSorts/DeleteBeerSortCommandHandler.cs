using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.BeerSorts
{
    public class DeleteBeerSortCommandHandler : IRequestHandler<DeleteBeerSortCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteBeerSortCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteBeerSortCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            // 1) delete BeerBatches, that assosiate with BeerSortiga
            await _dbContext
                .BeerBatches
                .Where(b => b.BeerSortId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // 2) delete BeerSort
            await _dbContext
                .BeerSorts
                .Where(s => s.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
