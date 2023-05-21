namespace Politicz.News;

public static class Routes
{
    public static WebApplication MapNews(this WebApplication app)
    {
        var newsGroup = app.MapGroup("/news");

        _ = newsGroup.MapGet(string.Empty, (INewsDbContext db) => Results.Ok(db.News));
        _ = newsGroup.MapPost("create", (INewsDbContext news, NewsEntity ent) =>
        {
            ent = news.News.Add(ent).Entity;
            _ = news.SaveChanges();
            return Results.Created($"/news/{ent.ExternalId}", ent);
        });
        _ = newsGroup.MapGet("{id:guid}", (INewsDbContext db, Guid id) =>
        {
            var news = db.News.AsNoTracking().FirstOrDefault(x => x.ExternalId == id);

            return news is null ? Results.NotFound() : Results.Ok(news);
        });

        _ = newsGroup.MapPut("{id:guid}", (INewsDbContext db, Guid id, NewsEntity updatedNews) =>
        {
            int result = db.News.Where(x => x.ExternalId == id)
                .ExecuteUpdate(s => s
                    .SetProperty(x => x.Heading, updatedNews.Heading)
                    .SetProperty(x => x.Content, updatedNews.Content)
                    .SetProperty(x => x.ImageUrl, updatedNews.ImageUrl));

            return result > 0 ? Results.Ok() : Results.NotFound();
        });

        return app;
    }
}
