using Application.DTOs.User;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserInputDTO>
    {
        public UpdateUserValidator(IUserRepository userRepository, ILevelRepository levelRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be a valid identifier.")
                .MustAsync(async (id, _) => await userRepository.ExistsByIdAsync(id))
                .WithMessage("User with given Id does not exist.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email is not valid.")
                .MustAsync(async (dto, email, _) =>
                    email == null || !await userRepository.IsEmailTakenByOtherUserAsync(email, dto.Id))
                .WithMessage("Email is already used by another user.")
                .When(x => x.Email != null);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .When(x => x.Name != null);

            RuleFor(x => x.Birthday)
                .LessThan(DateTime.Today).WithMessage("Birthday must be in the past.")
                .When(x => x.Birthday.HasValue);

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than zero.")
                .When(x => x.Weight.HasValue);

            RuleFor(x => x.Height)
                .GreaterThan(0).WithMessage("Height must be greater than zero.")
                .When(x => x.Height.HasValue);

            RuleFor(x => x.LevelId)
                .MustAsync(async (levelId, _) => levelId == null || await levelRepository.ExistsByIdAsync(levelId.Value))
                .WithMessage("LevelId does not exist.")
                .When(x => x.LevelId.HasValue);
        }
    }
}
