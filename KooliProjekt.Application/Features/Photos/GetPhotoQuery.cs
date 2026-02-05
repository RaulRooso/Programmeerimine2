using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Photos
{
    public class GetPhotoQuery : IRequest<OperationResult<PhotoDto>>
    {
        public int Id { get; set; }
    }
}