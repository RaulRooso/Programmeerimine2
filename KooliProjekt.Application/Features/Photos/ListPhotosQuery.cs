using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System.Collections.Generic;

namespace KooliProjekt.Application.Features.Photos
{
    public class ListPhotosQuery : IRequest<OperationResult<IList<Photo>>>
    {
    }
}
