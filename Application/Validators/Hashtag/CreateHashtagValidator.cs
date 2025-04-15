using Application.DTOs.Hashtag;
using FluentValidation;

namespace Application.Validators.Hashtag
{
    public class CreateHashtagValidator : AbstractValidator<CreateHashtagInputDTO>
    {
        public CreateHashtagValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
