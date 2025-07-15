using Application.DTOs.Video;
using FluentValidation;

namespace Application.Validators.Video
{
    public class CreateVideoValidator : AbstractValidator<CreateVideoInputDTO>
    {
        public CreateVideoValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.");

            RuleFor(x => x.File.FileName)
                .NotEmpty().WithMessage("File name is required.")
                .When(x => x.File != null);

            RuleFor(x => x.File.Length)
                .GreaterThan(0).WithMessage("File must not be empty.")
                .When(x => x.File != null);
        }
    }
}
