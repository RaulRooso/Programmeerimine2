using FluentValidation;
using KooliProjekt.Application.Features.BeerSorts;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Application.Validators
{
    [ExcludeFromCodeCoverage]
    public class SaveBeerSortValidator : AbstractValidator<SaveBeerSortCommand>
    {
        public SaveBeerSortValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
        }
    }
}
