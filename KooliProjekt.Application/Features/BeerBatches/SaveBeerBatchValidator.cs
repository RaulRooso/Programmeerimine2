using FluentValidation;
using KooliProjekt.Application.Features.BeerBatches;
using System;

namespace KooliProjekt.Application.Validators
{
    public class SaveBeerBatchValidator : AbstractValidator<SaveBeerBatchCommand>
    {
        public SaveBeerBatchValidator()
        {
            RuleFor(x => x.BeerSortId)
                .GreaterThan(0).WithMessage("BeerSortId must be provided");

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Date cannot be in the future");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.Conclusion)
                .MaximumLength(500).WithMessage("Conclusion cannot exceed 500 characters");
        }
    }
}
