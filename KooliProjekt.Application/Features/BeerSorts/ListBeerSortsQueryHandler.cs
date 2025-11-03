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
    public class ListBeerSortsQueryHandler : IRequestHandler<ListBeerSortsQuery, OperationResult<IList<BeerSort>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListBeerSortsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<BeerSort>>> Handle(ListBeerSortsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<BeerSort>>();
            Console.WriteLine("----");
            Console.WriteLine("sort list query handler enne p2ringu tegemist");
            Console.WriteLine("----");
            result.Value = await _dbContext
                .BeerSorts
                .OrderBy(b => b.Name)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
