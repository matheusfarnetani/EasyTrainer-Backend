using Application.DTOs;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.Instructor
{
    public class InstructorEmailValidator : AbstractValidator<EmailInputDTO>
    {
        public InstructorEmailValidator(IInstructorRepository instructorRepository)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email format is invalid.")
                .MustAsync(async (email, _) => await instructorRepository.ExistsByEmailAsync(email))
                .WithMessage("No instructor found with this email.");
        }
    }
}
