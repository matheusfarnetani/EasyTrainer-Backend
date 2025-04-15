using Application.DTOs.TrainingType;
using FluentValidation;

namespace Application.Validators.Type
{
    public class CreateTypeValidator : AbstractValidator<CreateTypeInputDTO>
    {
        public CreateTypeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
