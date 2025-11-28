using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class DeleteUserCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
