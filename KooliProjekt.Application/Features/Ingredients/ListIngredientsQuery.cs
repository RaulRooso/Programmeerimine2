using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.Ingredients
{
    public class ListIngredientsQuery : IRequest<OperationResult<IList<Ingredient>>>
    {
    }
}
