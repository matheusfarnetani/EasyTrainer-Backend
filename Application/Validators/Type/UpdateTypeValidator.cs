using Application.DTOs.TrainingType;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Validators.Type
{
    public class UpdateTypeValidator : AbstractValidator<UpdateTypeInputDTO>
    {
        public UpdateTypeValidator(ITypeRepository typeRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be valid.")
                .MustAsync(async (id, _) => await typeRepository.ExistsByIdAsync(id))
                .WithMessage("Type with the specified Id does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
                .When(x => x.Name != null);
        }
    }
}
