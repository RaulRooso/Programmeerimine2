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
        public async Task<IActionResult> List()
        {
            var query = new ListBatchLogsQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
