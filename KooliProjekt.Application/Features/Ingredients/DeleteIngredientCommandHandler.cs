using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Ingredients
{
    public class DeleteIngredientCommandHandler : IRequestHandler<DeleteIngredientCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteIngredientCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.Ingredients
                .Where(i => i.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
