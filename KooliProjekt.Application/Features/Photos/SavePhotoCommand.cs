using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Photos
{
    public class SavePhotoCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int BeerBatchId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
