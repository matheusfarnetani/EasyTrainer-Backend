using Application.DTOs.Routine;
using FluentValidation;

namespace Application.Validators.Routine
{
    public class CreateRoutineValidator : AbstractValidator<CreateRoutineInputDTO>
    {
        public CreateRoutineValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Routine name is required.")
                .MaximumLength(100).WithMessage("Routine name must be at most 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must be at most 500 characters.")
                .When(x => x.Description != null);
        }
    }
}
