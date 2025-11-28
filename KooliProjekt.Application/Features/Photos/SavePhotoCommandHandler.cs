using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Photos
{
    public class SavePhotoCommandHandler : IRequestHandler<SavePhotoCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SavePhotoCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SavePhotoCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var photo = new Photo();
            if (request.Id == 0)
            {
                await _dbContext.Photos.AddAsync(photo, cancellationToken);
            }
            else
            {
                photo = await _dbContext.Photos.FindAsync(new object[] { request.Id }, cancellationToken);
            }

            photo.BeerBatchId = request.BeerBatchId;
            photo.FilePath = request.FilePath;
            photo.Description = request.Description;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
