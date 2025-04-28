using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class EmailInputValidator : AbstractValidator<EmailInputDTO>
    {
        public EmailInputValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
