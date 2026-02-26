using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Photos
{
    [ExcludeFromCodeCoverage]
    public class SavePhotoCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int BeerBatchId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
