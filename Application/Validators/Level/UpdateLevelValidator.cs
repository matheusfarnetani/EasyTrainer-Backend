using Application.DTOs.Level;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Validators.Level
{
    public class UpdateLevelValidator : AbstractValidator<UpdateLevelInputDTO>
    {
        public UpdateLevelValidator(ILevelRepository levelRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be valid.")
                .MustAsync(async (id, _) => await levelRepository.ExistsByIdAsync(id))
                .WithMessage("Level with specified Id does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
                .When(x => x.Name != null);
        }
    }
}
