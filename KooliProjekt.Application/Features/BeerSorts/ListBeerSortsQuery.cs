using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.BeerSorts
{
    [ExcludeFromCodeCoverage]
    public class ListBeerSortsQuery : IRequest<OperationResult<PagedResult<BeerSort>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
    }
}
