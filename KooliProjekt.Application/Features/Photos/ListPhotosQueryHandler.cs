using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Photos
{
    public class ListPhotosQueryHandler : IRequestHandler<ListPhotosQuery, OperationResult<object>>
    {
        private readonly IPhotoRepository _photoRepository;

        public ListPhotosQueryHandler(IPhotoRepository photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task<OperationResult<object>> Handle(ListPhotosQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var list = await _photoRepository.ListAsync();
            result.Value = list;

            return result;
        }
    }
}
