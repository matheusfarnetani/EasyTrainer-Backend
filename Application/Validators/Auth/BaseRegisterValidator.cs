using FluentValidation;
using Application.DTOs.Auth;

namespace Application.Validators.Auth
{
    public abstract class BaseRegisterValidator<T> : AbstractValidator<T> where T : BaseRegisterDTO
    {
        protected BaseRegisterValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(255);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}