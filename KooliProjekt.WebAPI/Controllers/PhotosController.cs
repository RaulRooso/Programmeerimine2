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
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListPhotosQuery query)
        {
            var result = await _mediator.Send(query);
            return Result(result);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] DeletePhotoCommand command)
        {
            var result = await _mediator.Send(command);
            return Result(result);
        }
    }
}
