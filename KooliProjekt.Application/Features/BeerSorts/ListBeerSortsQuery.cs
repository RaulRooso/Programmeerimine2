using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.BeerSorts
{
    public class ListBeerSortsQuery : IRequest<OperationResult<IList<BeerSort>>>
    {
    }
}
