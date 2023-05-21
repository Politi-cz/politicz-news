namespace Politicz.News.Models;

public abstract class Entity
{
    public int Id { get; private set; }

    public Guid ExternalId { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public DateTime? UpdatedOn { get; private set; }
}
