using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Photos
{
    public class GetPhotoQueryHandler : IRequestHandler<GetPhotoQuery, OperationResult<PhotoDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetPhotoQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<OperationResult<PhotoDto>> Handle(GetPhotoQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult<PhotoDto>();
            if (request.Id <= 0) return result;

            result.Value = await _dbContext.Photos
                .Where(x => x.Id == request.Id)
                .Select(x => new PhotoDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    FilePath = x.FilePath,
                    BeerBatchId = x.BeerBatchId
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}