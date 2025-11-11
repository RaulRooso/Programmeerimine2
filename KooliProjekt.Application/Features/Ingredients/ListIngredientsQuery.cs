using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.Ingredients
{
    public class ListIngredientsQuery : IRequest<OperationResult<PagedResult<Ingredient>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
