using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Photos
{
    public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, OperationResult>
    {
        private readonly IPhotoRepository _photoRepository;

        public DeletePhotoCommandHandler(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task<OperationResult> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var photo = await _photoRepository.GetByIdAsync(request.Id);
            if (photo != null)
            {
                await _photoRepository.DeleteAsync(photo);
            }

            return result;
        }
    }
}
