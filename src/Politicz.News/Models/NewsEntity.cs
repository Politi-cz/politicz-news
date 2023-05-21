namespace Politicz.News.Models;

public sealed class NewsEntity : Entity
{
    public NewsEntity(string heading, string content, string imageUrl)
    {
        Heading = heading;
        Content = content;
        ImageUrl = imageUrl;
    }

    public string Heading { get; private set; }

    public string Content { get; private set; }

    public string ImageUrl { get; private set; }
}
