namespace Politicz.News.Contracts;

public sealed record NewsResponse(
    Guid Id,
    string Heading,
    string Content,
    string ImageUrl,
    DateTime PublishDate);
