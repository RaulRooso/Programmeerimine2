using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Ingredients
{
    public class SaveIngredientCommandHandler : IRequestHandler<SaveIngredientCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveIngredientCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveIngredientCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var ingredient = new Ingredient();
            if (request.Id == 0)
            {
                await _dbContext.Ingredients.AddAsync(ingredient, cancellationToken);
            }
            else
            {
                ingredient = await _dbContext.Ingredients.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            ingredient.BeerBatchId = request.BeerBatchId;
            ingredient.Name = request.Name;
            ingredient.Unit = request.Unit;
            ingredient.Quantity = request.Quantity;
            ingredient.UnitPrice = request.UnitPrice;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
