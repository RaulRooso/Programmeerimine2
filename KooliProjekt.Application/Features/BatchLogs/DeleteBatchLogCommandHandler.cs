using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.BatchLogs
{
    public class DeleteBatchLogCommandHandler : IRequestHandler<DeleteBatchLogCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteBatchLogCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteBatchLogCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.BatchLogs
                .Where(l => l.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
