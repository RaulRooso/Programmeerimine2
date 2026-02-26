using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Features.Ingredients
{
    [ExcludeFromCodeCoverage]
    public class SaveIngredientCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public int BeerBatchId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
