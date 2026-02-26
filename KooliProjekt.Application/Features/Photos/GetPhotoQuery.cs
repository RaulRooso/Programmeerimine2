using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Photos
{
    [ExcludeFromCodeCoverage]
    public class GetPhotoQuery : IRequest<OperationResult<PhotoDto>>
    {
        public int Id { get; set; }
    }
}