using FluentValidation;
using KooliProjekt.Application.Features.Users;

namespace KooliProjekt.Application.Validators
{
    public class SaveUserValidator : AbstractValidator<SaveUserCommand>
    {
        public SaveUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email must be a valid email address")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Email)); // only validate if provided
        }
    }
}
