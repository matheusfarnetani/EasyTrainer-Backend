using Application.DTOs.Modality;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Validators.Modality
{
    public class UpdateModalityValidator : AbstractValidator<UpdateModalityInputDTO>
    {
        public UpdateModalityValidator(IModalityRepository repository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be a valid identifier.")
                .MustAsync(async (id, _) => await repository.ExistsByIdAsync(id))
                .WithMessage("Modality with the specified Id does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
                .When(x => x.Name != null);
        }
    }
}
