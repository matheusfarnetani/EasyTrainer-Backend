using Application.DTOs;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.Level
{
    public class LevelIdValidator : AbstractValidator<IdInputDTO>
    {
        public LevelIdValidator(ILevelRepository repository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, _) => await repository.ExistsByIdAsync(id))
                .WithMessage("Level with the specified Id does not exist.");
        }
    }
}
