using FluentValidation;
using KooliProjekt.Application.Features.BatchLogs;
using System;

namespace KooliProjekt.Application.Validators
{
    public class SaveBatchLogValidator : AbstractValidator<SaveBatchLogCommand>
    {
        public SaveBatchLogValidator()
        {
            RuleFor(x => x.BeerBatchId)
                .GreaterThan(0).WithMessage("BeerBatchId must be provided");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be provided");

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Date cannot be in the future");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
        }
    }
}
