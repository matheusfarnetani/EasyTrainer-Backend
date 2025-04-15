using Application.DTOs;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Validators.Modality
{
    public class ModalityIdValidator : AbstractValidator<IdInputDTO>
    {
        public ModalityIdValidator(IModalityRepository repository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .MustAsync(async (id, _) => await repository.ExistsByIdAsync(id))
                .WithMessage("Modality with the specified Id does not exist.");
        }
    }
}
