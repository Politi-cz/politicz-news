namespace Politicz.News.Validations;

public class UpdateNewsValidator : AbstractValidator<UpdateNewsCommand>
{
    public UpdateNewsValidator()
    {
        _ = RuleFor(x => x.News.Heading).NotEmpty().MaximumLength(255);
        _ = RuleFor(x => x.News.Content).NotEmpty().MaximumLength(10000);
        _ = RuleFor(x => x.News.ImageUrl).NotEmpty();
    }
}
