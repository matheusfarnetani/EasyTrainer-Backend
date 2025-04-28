using Application.DTOs;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.Routine
{
    public class RoutineIdValidator : AbstractValidator<IdInputDTO>
    {
        public RoutineIdValidator(IRoutineRepository routineRepository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, _) => await routineRepository.ExistsByIdAsync(id))
                .WithMessage("Routine with the specified Id does not exist.");
        }
    }
}
