using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Ingredients
{
    public class ListIngredientsQueryHandler : IRequestHandler<ListIngredientsQuery, OperationResult<PagedResult<Ingredient>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListIngredientsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Ingredient>>> Handle(ListIngredientsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<Ingredient>>();
            result.Value = await _dbContext
                .Ingredients
                .OrderBy(i => i.Name)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
