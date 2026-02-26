using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Users
{
    [ExcludeFromCodeCoverage]
    public class ListUsersQuery : IRequest<OperationResult<PagedResult<User>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Username { get; set; }
    }
}
