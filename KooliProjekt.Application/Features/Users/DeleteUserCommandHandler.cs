using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Users
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
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
            var item = await _dbContext.Users
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (item == null)
            {
                return result;
            }

            // Remove it from the change tracker
            _dbContext.Users.Remove(item);

            // Persist changes
            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
