using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class IdInputValidator : AbstractValidator<IdInputDTO>
    {
        public IdInputValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
