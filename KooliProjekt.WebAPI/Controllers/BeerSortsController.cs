using System.Threading.Tasks;
using KooliProjekt.Application.Features.BeerSorts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class BeerSortsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public BeerSortsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListBeerSortsQuery query)
        {
            var result = await _mediator.Send(query);
            return Result(result);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteBeerSortCommand command)
        {
            var result = await _mediator.Send(command);
            return Result(result);
        }
    }
}
