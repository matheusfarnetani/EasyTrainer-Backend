using Application.DTOs.Hashtag;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.Hashtag
{
    public class UpdateHashtagValidator : AbstractValidator<UpdateHashtagInputDTO>
    {
        public UpdateHashtagValidator(IHashtagRepository repository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .MustAsync(async (id, _) => await repository.ExistsByIdAsync(id))
                .WithMessage("Hashtag with the specified Id does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
                .When(x => x.Name != null);
        }
    }
}
