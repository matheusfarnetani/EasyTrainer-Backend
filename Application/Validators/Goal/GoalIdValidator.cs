using Application.DTOs;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Validators.Goal
{
    public class GoalIdValidator : AbstractValidator<IdInputDTO>
    {
        public GoalIdValidator(IGoalRepository repository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, _) => await repository.ExistsByIdAsync(id))
                .WithMessage("Goal with the specified Id does not exist.");
        }
    }
}
