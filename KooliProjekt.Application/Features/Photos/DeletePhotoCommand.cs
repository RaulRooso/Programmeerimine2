using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Photos
{
    [ExcludeFromCodeCoverage]
    public class DeletePhotoCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
