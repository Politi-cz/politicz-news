namespace Politicz.News.Test.Integration.Setup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTestSecurity(this IServiceCollection services)
    {
        _ = services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = TestAuthenticationHandler.AuthenticationScheme;
                o.DefaultScheme = TestAuthenticationHandler.AuthenticationScheme;
                o.DefaultChallengeScheme = TestAuthenticationHandler.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                TestAuthenticationHandler.AuthenticationScheme, _ => { });

        _ = services.AddAuthorization(con =>
            con.AddPolicy(AuthConstants.ModifyNewsPolicy, p
                =>
            {
                _ = p.AddAuthenticationSchemes(TestAuthenticationHandler.AuthenticationScheme);
                _ = p.RequireClaim("permissions", AuthConstants.ModifyNewsPermission);
            }));

        return services;
    }

    public static IServiceCollection AddTestDatabase(
        this IServiceCollection services,
        string connectionString) =>
        services.
            RemoveAll<NewsDbContext>()
            .AddScoped<NewsDbContext>(_ =>
            {
                var databaseOptions =
                    Microsoft.Extensions.Options.Options.Create(
                        new DatabaseOptions { ConnectionString = connectionString, });

                return new NewsDbContext(databaseOptions);
            });
}
