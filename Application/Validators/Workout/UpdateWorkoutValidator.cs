using Application.DTOs.Workout;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.Workout
{
    public class UpdateWorkoutValidator : AbstractValidator<UpdateWorkoutInputDTO>
    {
        public UpdateWorkoutValidator(IWorkoutRepository workoutRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .MustAsync(async (id, _) => await workoutRepository.ExistsByIdAsync(id))
                .WithMessage("Workout with the specified Id does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Workout name is required.")
                .MaximumLength(100).WithMessage("Workout name must be at most 100 characters.")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must be at most 500 characters.")
                .When(x => x.Description != null);
        }
    }
}
