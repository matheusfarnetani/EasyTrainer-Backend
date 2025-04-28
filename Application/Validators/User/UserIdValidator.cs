using Application.DTOs;
using Domain.Infrastructure.RepositoriesInterfaces;
using FluentValidation;

namespace Application.Validators.User
{
    public class UserIdValidator : AbstractValidator<IdInputDTO>
    {
        public UserIdValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, _) => await userRepository.ExistsByIdAsync(id))
                .WithMessage("User with the specified Id does not exist.");
        }
    }
}
