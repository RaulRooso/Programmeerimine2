using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var user = new User();
            if (request.Id == 0)
            {
                await _dbContext.Users.AddAsync(user, cancellationToken);
            }
            else
            {
                user = await _dbContext.Users.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            user.Username = request.Username;
            user.Email = request.Email;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
