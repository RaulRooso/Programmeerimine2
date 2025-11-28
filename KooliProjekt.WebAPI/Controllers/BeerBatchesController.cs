using System.Threading.Tasks;
using KooliProjekt.Application.Features.BeerBatches;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class BeerBatchesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public BeerBatchesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListBeerBatchesQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteBeerBatchCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}
