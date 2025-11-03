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
        public async Task<IActionResult> List()
        {
            var query = new ListBeerBatchesQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
