using System.Threading.Tasks;
using KooliProjekt.Application.Features.Ingredients;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class IngredientsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public IngredientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var query = new ListIngredientsQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
