using FluentValidation;
using KooliProjekt.Application.Features.Ingredients;

namespace KooliProjekt.Application.Validators
{
    public class SaveIngredientValidator : AbstractValidator<SaveIngredientCommand>
    {
        public SaveIngredientValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage("Unit is required")
                .MaximumLength(20).WithMessage("Unit cannot exceed 20 characters");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be non-negative");

            RuleFor(x => x.BeerBatchId)
                .GreaterThan(0).WithMessage("BeerBatchId must be provided");
        }
    }
}
