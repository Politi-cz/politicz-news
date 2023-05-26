namespace Politicz.News.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
        => services
            .AddScoped<NewsDbContext>()
            .AddScoped<INewsDbContext>(sp => sp.GetRequiredService<NewsDbContext>());
}
