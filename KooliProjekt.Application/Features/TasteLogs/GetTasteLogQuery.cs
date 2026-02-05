using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TasteLogs
{
    public class GetTasteLogQuery : IRequest<OperationResult<TasteLogDto>>
    {
        public int Id { get; set; }
    }
}