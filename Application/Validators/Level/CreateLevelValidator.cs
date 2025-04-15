using Application.DTOs.Level;
using FluentValidation;

namespace Application.Validators.Level
{
    public class CreateLevelValidator : AbstractValidator<CreateLevelInputDTO>
    {
        public CreateLevelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
