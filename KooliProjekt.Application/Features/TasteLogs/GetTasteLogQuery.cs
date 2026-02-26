using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.TasteLogs
{
    [ExcludeFromCodeCoverage]
    public class GetTasteLogQuery : IRequest<OperationResult<TasteLogDto>>
    {
        public int Id { get; set; }
    }
}