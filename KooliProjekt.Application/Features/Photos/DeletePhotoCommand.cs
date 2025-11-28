using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Photos
{
    public class DeletePhotoCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
