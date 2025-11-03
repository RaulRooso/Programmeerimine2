using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.Users
{
    public class ListUsersQuery : IRequest<OperationResult<IList<User>>>
    {
    }
}
