namespace Politicz.News;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSecurity(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        _ = services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetRequiredSection("JwtSettings").Get<JwtOptions>();
                options.Authority = jwtSettings!.Authority;
                options.Audience = jwtSettings.Audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                };
            });

        _ = services.AddAuthorization(o =>
            o.AddPolicy(
                AuthConstants.ModifyNewsPolicy,
                p => p
                    .RequireClaim("permissions", AuthConstants.ModifyNewsPermission)
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)));

        _ = services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        return services;
    }
}
