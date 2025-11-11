using System.Threading.Tasks;
using KooliProjekt.Application.Features.Photos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class PhotosController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PhotosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListPhotosQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }
    }
}
