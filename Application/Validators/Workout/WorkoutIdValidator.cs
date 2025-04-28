using Application.DTOs;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.Workout
{
    public class WorkoutIdValidator : AbstractValidator<IdInputDTO>
    {
        public WorkoutIdValidator(IWorkoutRepository repository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, _) => await repository.ExistsByIdAsync(id))
                .WithMessage("Workout with the specified Id does not exist.");
        }
    }
}
