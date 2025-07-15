using Application.DTOs.Video;
using FluentValidation;
using Domain.Infrastructure.RepositoriesInterfaces;

namespace Application.Validators.Video
{
    public class UpdateVideoValidator : AbstractValidator<UpdateVideoInputDTO>
    {
        public UpdateVideoValidator(IVideoRepository videoRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be a valid identifier.")
                .MustAsync(async (id, _) => await videoRepository.ExistsByIdAsync(id))
                .WithMessage("Video with the specified Id does not exist.");

            RuleFor(x => x.Filename)
                .MaximumLength(255)
                .When(x => x.Filename != null);

            RuleFor(x => x.Status)
                .InclusiveBetween(0, 3)
                .When(x => x.Status.HasValue);
        }
    }
}
