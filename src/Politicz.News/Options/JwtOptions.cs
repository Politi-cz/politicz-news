namespace Politicz.News.Options;

public sealed class JwtOptions
{
    public required string Authority { get; init; }

    public required string Audience { get; init; }
}
