using Application.DTOs.User;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserInputDTO>
    {
        public CreateUserValidator(IUserRepository userRepository, ILevelRepository levelRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.")
                .MustAsync(async (email, _) => !await userRepository.ExistsByEmailAsync(email))
                .WithMessage("Email is already in use.");

            RuleFor(x => x.MobileNumber)
                .MaximumLength(15).WithMessage("Mobile number must be at most 15 characters.");

            RuleFor(x => x.Birthday)
                .LessThan(DateTime.Today).WithMessage("Birthday must be a date in the past.");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than zero.");

            RuleFor(x => x.Height)
                .GreaterThan(0).WithMessage("Height must be greater than zero.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.LevelId)
                .MustAsync(async (levelId, _) => await levelRepository.ExistsAsync(levelId))
                .WithMessage("LevelId does not exist.");
        }
    }
}
