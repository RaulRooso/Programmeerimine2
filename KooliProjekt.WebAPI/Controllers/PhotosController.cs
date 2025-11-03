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
        public async Task<IActionResult> List()
        {
            var query = new ListPhotosQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
