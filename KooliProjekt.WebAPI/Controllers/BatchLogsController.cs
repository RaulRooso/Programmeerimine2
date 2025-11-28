using System.Threading.Tasks;
using KooliProjekt.Application.Features.BatchLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class BatchLogsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public BatchLogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListBatchLogsQuery query)
        {
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] DeleteBatchLogCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}
