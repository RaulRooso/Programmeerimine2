using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.TasteLogs
{
    public class DeleteTasteLogCommandHandler : IRequestHandler<DeleteTasteLogCommand, OperationResult>
    {
        private readonly ITasteLogRepository _tasteLogRepository;

        public DeleteTasteLogCommandHandler(ITasteLogRepository tasteLogRepository)
        {
            _tasteLogRepository = tasteLogRepository;
        }

        public async Task<OperationResult> Handle(DeleteTasteLogCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var tasteLog = await _tasteLogRepository.GetByIdAsync(request.Id);
            if (tasteLog != null)
            {
                await _tasteLogRepository.DeleteAsync(tasteLog);
            }

            return result;
        }
    }
}
