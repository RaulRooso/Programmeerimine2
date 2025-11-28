using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TasteLogs
{
    public class DeleteTasteLogCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
    }
}
