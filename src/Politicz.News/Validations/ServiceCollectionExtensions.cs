namespace Politicz.News.Validations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
        => services
            .AddScoped<IPipelineBehavior<CreateNewsCommand, OneOf<NewsEntity, Failure>>,
                ValidationBehavior<CreateNewsCommand, NewsEntity>>()
            .AddScoped<IPipelineBehavior<UpdateNewsCommand, OneOf<NewsEntity, NotFound, Failure>>,
                ValidationBehavior<UpdateNewsCommand, NewsEntity>>()
            .AddValidatorsFromAssemblyContaining<CreateNewsValidator>();
}
