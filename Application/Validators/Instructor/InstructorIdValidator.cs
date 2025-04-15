using Application.DTOs;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Validators.Instructor
{
    public class InstructorIdValidator : AbstractValidator<IdInputDTO>
    {
        public InstructorIdValidator(IInstructorRepository repository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, _) => await repository.ExistsByIdAsync(id))
                .WithMessage("Instructor with the specified Id does not exist.");
        }
    }
}
