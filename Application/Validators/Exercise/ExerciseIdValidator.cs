using Application.DTOs;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Validators.Exercise
{
    public class ExerciseIdValidator : AbstractValidator<IdInputDTO>
    {
        public ExerciseIdValidator(IExerciseRepository exerciseRepository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, _) => await exerciseRepository.ExistsByIdAsync(id))
                .WithMessage("Exercise with the specified Id does not exist.");
        }
    }
}
