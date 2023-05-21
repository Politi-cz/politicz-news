namespace Politicz.News.Mappings;

public static class DtoToEntity
{
    public static NewsEntity ToEntity(this CreateNews news) =>
        new(news.Heading, news.Content, news.ImageUrl);
}
