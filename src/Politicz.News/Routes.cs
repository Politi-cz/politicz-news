using Politicz.News.Features.Queries;

namespace Politicz.News;

public static class Routes
{
    public static WebApplication MapNews(this WebApplication app)
    {
        var newsGroup = app.MapGroup("/news");

        _ = newsGroup.MapGet(string.Empty, async (IMediator mediator, CancellationToken token) =>
        {
            var news = await mediator.Send(new GetAllNewsQuery(), token);
            return Results.Ok(news.Select(x => x.ToResponse()));
        });

        _ = newsGroup.MapPost("create", async (IMediator mediator, CreateNews news) =>
        {
            var result =
                await mediator.Send(new CreateNewsCommand(news));

            return result.Match(
                created => Results.Created($"/news/{created.ExternalId}", created.ToResponse()),
                Results.BadRequest);
        });

        _ = newsGroup.MapGet("{id:guid}", async (IMediator mediator, Guid id, CancellationToken token) =>
        {
            var result = await mediator.Send(new GetNewsQuery(id), token);

            return result.Match(
                news => Results.Ok(news.ToResponse()),
                _ => Results.NotFound());
        });

        _ = newsGroup.MapPut("{id:guid}", async (IMediator mediator, Guid id, UpdateNews updateNews) =>
        {
            var result = await mediator.Send(new UpdateNewsCommand(id, updateNews));

            return result.Match(
                updated => Results.Ok(updated.ToResponse()),
                _ => Results.NotFound(),
                Results.BadRequest);
        });

        _ = newsGroup.MapDelete("{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var result = await mediator.Send(new DeleteNewsCommand(id));

            return result.Match(
                _ => Results.Ok(),
                _ => Results.NotFound());
        });

        return app;
    }
}
