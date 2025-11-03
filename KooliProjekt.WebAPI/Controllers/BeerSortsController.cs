using System;
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
        public async Task<IActionResult> List()
        {
            Console.WriteLine("---");
            Console.WriteLine("Joudsime beer list controllerisse");
            Console.WriteLine("----");
            var query = new ListBeerSortsQuery();
            var result = await _mediator.Send(query);

            Console.WriteLine("----");
            Console.WriteLine("Beer list controlleris p2rast vastust");
            Console.WriteLine("----");

            return Result(result);
        }
    }
}
