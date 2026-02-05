using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            //Find the item first
            var item = await _dbContext.Ingredients
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (item == null)
            {
                return result;
            }

            // Remove it from the change tracker
            _dbContext.Ingredients.Remove(item);

            // Persist changes
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
