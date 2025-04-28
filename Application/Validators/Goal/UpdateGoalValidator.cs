using Application.DTOs.Goal;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.Goal
{
    public class UpdateGoalValidator : AbstractValidator<UpdateGoalInputDTO>
    {
        public UpdateGoalValidator(IGoalRepository goalRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be a valid identifier.")
                .MustAsync(async (id, _) => await goalRepository.ExistsByIdAsync(id))
                .WithMessage("Goal with the specified Id does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.")
                .When(x => x.Description != null);
        }
    }
}
