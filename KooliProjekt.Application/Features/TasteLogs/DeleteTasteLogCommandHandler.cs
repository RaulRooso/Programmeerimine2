using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.TasteLogs
{
    public class DeleteTasteLogCommandHandler : IRequestHandler<DeleteTasteLogCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteTasteLogCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteTasteLogCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.TasteLogs
                .Where(t => t.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
