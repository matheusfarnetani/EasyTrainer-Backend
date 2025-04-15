using Application.DTOs;
using Domain.RepositoryInterfaces;
using FluentValidation;

namespace Application.Validators.Hashtag
{
    public class HashtagIdValidator : AbstractValidator<IdInputDTO>
    {
        public HashtagIdValidator(IHashtagRepository repository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .MustAsync(async (id, _) => await repository.ExistsByIdAsync(id))
                .WithMessage("Hashtag with the specified Id does not exist.");
        }
    }
}
