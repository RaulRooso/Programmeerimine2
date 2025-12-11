using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Features.TasteLogs
{
    public class SaveTasteLogCommandHandler : IRequestHandler<SaveTasteLogCommand, OperationResult>
    {
        private readonly ITasteLogRepository _tasteLogRepository;

        public SaveTasteLogCommandHandler(ITasteLogRepository tasteLogRepository)
        {
            _tasteLogRepository = tasteLogRepository;
        }

        public async Task<OperationResult> Handle(SaveTasteLogCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var tasteLog = new TasteLog();
            if (request.Id != 0)
            {
                tasteLog = await _tasteLogRepository.GetByIdAsync(request.Id);
            }

            tasteLog.Date = request.Date;
            tasteLog.Description = request.Description;
            tasteLog.Rating = request.Rating;
            tasteLog.UserId = request.UserId;
            tasteLog.BeerBatchId = request.BeerBatchId;

            await _tasteLogRepository.SaveAsync(tasteLog);

            return result;
        }
    }
}
