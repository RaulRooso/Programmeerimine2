using FluentValidation;
using KooliProjekt.Application.Features.Photos;

namespace KooliProjekt.Application.Validators
{
    public class SavePhotoValidator : AbstractValidator<SavePhotoCommand>
    {
        public SavePhotoValidator()
        {
            RuleFor(x => x.BeerBatchId)
                .GreaterThan(0).WithMessage("BeerBatchId must be provided");

            RuleFor(x => x.FilePath)
                .NotEmpty().WithMessage("FilePath is required")
                .MaximumLength(260).WithMessage("FilePath cannot exceed 260 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
        }
    }
}
