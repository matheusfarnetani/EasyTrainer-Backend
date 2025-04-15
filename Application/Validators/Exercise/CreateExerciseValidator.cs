using Application.DTOs.Exercise;
using FluentValidation;

namespace Application.Validators.Exercise
{
    public class CreateExerciseValidator : AbstractValidator<CreateExerciseInputDTO>
    {
        public CreateExerciseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Exercise name is required.")
                .MaximumLength(100).WithMessage("Exercise name must be at most 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must be at most 500 characters.")
                .When(x => x.Description != null);
        }
    }
}
