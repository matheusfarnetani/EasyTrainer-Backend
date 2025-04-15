using Application.DTOs.Modality;
using FluentValidation;

namespace Application.Validators.Modality
{
    public class CreateModalityValidator : AbstractValidator<CreateModalityInputDTO>
    {
        public CreateModalityValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
