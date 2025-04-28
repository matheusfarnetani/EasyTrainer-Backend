using Application.DTOs.Instructor;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.Instructor
{
    public class UpdateInstructorValidator : AbstractValidator<UpdateInstructorInputDTO>
    {
        public UpdateInstructorValidator(IInstructorRepository instructorRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be a valid identifier.")
                .MustAsync(async (id, _) =>
                    await instructorRepository.ExistsByIdAsync(id))
                .WithMessage("Instructor with given Id does not exist.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email is not valid.")
                .MustAsync(async (dto, email, _) =>
                    email == null || !await instructorRepository.IsEmailTakenByOtherAsync(email, dto.Id))
                .WithMessage("Email is already used by another instructor.")
                .When(x => x.Email != null);

            RuleFor(x => x.Birthday)
                .LessThan(DateTime.Today).WithMessage("Birthday must be a date in the past.")
                .When(x => x.Birthday.HasValue);
        }
    }
}
