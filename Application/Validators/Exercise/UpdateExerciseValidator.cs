using Application.DTOs.Exercise;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.Exercise
{
    public class UpdateExerciseValidator : AbstractValidator<UpdateExerciseInputDTO>
    {
        public UpdateExerciseValidator(IExerciseRepository repository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .MustAsync(async (id, _) => await repository.ExistsByIdAsync(id))
                .WithMessage("Exercise with the specified Id does not exist.");

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Exercise name must be at most 100 characters.")
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must be at most 500 characters.")
                .When(x => x.Description != null);
        }
    }
}
