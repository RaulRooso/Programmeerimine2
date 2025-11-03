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
        public async Task<IActionResult> List()
        {
            var query = new ListTasteLogsQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
