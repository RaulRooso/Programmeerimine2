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
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListIngredientsQuery query)
        {
            var result = await _mediator.Send(query);
            return Result(result);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteIngredientCommand command)
        {
            var result = await _mediator.Send(command);
            return Result(result);
        }
    }
}
