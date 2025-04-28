using Application.DTOs.Instructor;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.Instructor
{
    public class CreateInstructorValidator : AbstractValidator<CreateInstructorInputDTO>
    {
        public CreateInstructorValidator(IInstructorRepository instructorRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.")
                .MustAsync(async (email, _) =>
                    !await instructorRepository.ExistsByEmailAsync(email))
                .WithMessage("Email is already in use.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");

            RuleFor(x => x.Birthday)
                .LessThan(DateTime.Today).WithMessage("Birthday must be a date in the past.")
                .When(x => x.Birthday != default);
        }
    }
}
