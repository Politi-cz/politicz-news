namespace Politicz.News.Mappings;

public static class EntityToDto
{
    public static NewsResponse ToResponse(this NewsEntity newsEntity) => new(
            newsEntity.ExternalId,
            newsEntity.Heading,
            newsEntity.Content,
            newsEntity.ImageUrl,
            newsEntity.CreatedOn);
}
