using System.Threading.Tasks;
using KooliProjekt.Application.Features.TasteLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class TasteLogsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public TasteLogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListTasteLogsQuery query)
        {
            var result = await _mediator.Send(query);
            return Result(result);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteTasteLogCommand command)
        {
            var result = await _mediator.Send(command);
            return Result(result);
        }
    }
}
