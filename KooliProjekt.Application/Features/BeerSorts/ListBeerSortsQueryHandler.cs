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

namespace KooliProjekt.Application.Features.BeerSorts
{
    public class ListBeerSortsQueryHandler : IRequestHandler<ListBeerSortsQuery, OperationResult<PagedResult<BeerSort>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListBeerSortsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<BeerSort>>> Handle(ListBeerSortsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<BeerSort>>();
            Console.WriteLine("----");
            Console.WriteLine("sort list query handler enne p2ringu tegemist");
            Console.WriteLine("----");
            result.Value = await _dbContext
                .BeerSorts
                .OrderBy(b => b.Name)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
