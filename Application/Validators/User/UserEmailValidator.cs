using Application.DTOs;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Validators.User
{
    public class UserEmailValidator : AbstractValidator<EmailInputDTO>
    {
        public UserEmailValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email format is invalid.")
                .MustAsync(async (email, _) => await userRepository.ExistsByEmailAsync(email))
                .WithMessage("No user found with this email.");
        }
    }
}
