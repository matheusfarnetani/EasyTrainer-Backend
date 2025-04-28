using Application.DTOs;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.Type
{
    public class TypeIdValidator : AbstractValidator<IdInputDTO>
    {
        public TypeIdValidator(ITypeRepository typeRepository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, _) => await typeRepository.ExistsByIdAsync(id))
                .WithMessage("Routine with the specified Id does not exist.");
        }
    }
}
