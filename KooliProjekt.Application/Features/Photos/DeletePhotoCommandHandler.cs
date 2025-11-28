using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Photos
{
    public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeletePhotoCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            await _dbContext.Photos
                .Where(p => p.Id == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return result;
        }
    }
}
