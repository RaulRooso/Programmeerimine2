using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.Photos
{
    public class SavePhotoCommandHandler : IRequestHandler<SavePhotoCommand, OperationResult>
    {
        private readonly IPhotoRepository _photoRepository;

        public SavePhotoCommandHandler(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task<OperationResult> Handle(SavePhotoCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var photo = new Photo();
            if (request.Id != 0)
            {
                photo = await _photoRepository.GetByIdAsync(request.Id);
            }

            photo.Description = request.Description;
            photo.FilePath = request.FilePath;
            photo.BeerBatchId = request.BeerBatchId;

            await _photoRepository.SaveAsync(photo);

            return result;
        }
    }
}
