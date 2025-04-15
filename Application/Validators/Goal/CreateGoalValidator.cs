using Application.DTOs.Goal;
using FluentValidation;

namespace Application.Validators.Goal
{
    public class CreateGoalValidator : AbstractValidator<CreateGoalInputDTO>
    {
        public CreateGoalValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.")
                .When(x => x.Description != null);
        }
    }
}
