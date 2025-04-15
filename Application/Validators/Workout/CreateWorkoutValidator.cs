using Application.DTOs.Workout;
using FluentValidation;

namespace Application.Validators.Workout
{
    public class CreateWorkoutValidator : AbstractValidator<CreateWorkoutInputDTO>
    {
        public CreateWorkoutValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Workout name is required.")
                .MaximumLength(100).WithMessage("Workout name must be at most 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must be at most 500 characters.")
                .When(x => x.Description != null);
        }
    }
}
