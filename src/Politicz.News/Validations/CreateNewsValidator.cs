namespace Politicz.News.Validations;

public class CreateNewsValidator : AbstractValidator<CreateNewsCommand>
{
    public CreateNewsValidator()
    {
        _ = RuleFor(x => x.NewsDto.Heading).NotEmpty().MaximumLength(255);
        _ = RuleFor(x => x.NewsDto.Content).NotEmpty().MaximumLength(10000);
        _ = RuleFor(x => x.NewsDto.ImageUrl).NotEmpty();
    }
}
