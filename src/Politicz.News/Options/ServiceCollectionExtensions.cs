namespace Politicz.News.Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNewsOptions(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .Configure<DatabaseOptions>(configuration.GetRequiredSection("Database"));
}
