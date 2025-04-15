using Application.DTOs.Routine;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Validators.Routine
{
    public class UpdateRoutineValidator : AbstractValidator<UpdateRoutineInputDTO>
    {
        public UpdateRoutineValidator(IRoutineRepository routineRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .MustAsync(async (id, _) => await routineRepository.ExistsByIdAsync(id))
                .WithMessage("Routine with the specified Id does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Routine name is required.")
                .MaximumLength(100).WithMessage("Routine name must be at most 100 characters.")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must be at most 500 characters.")
                .When(x => x.Description != null);
        }
    }
}
