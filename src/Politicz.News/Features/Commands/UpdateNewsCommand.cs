namespace Politicz.News.Features.Commands;

public sealed record UpdateNewsCommand(Guid Id, UpdateNews News) : IRequest<OneOf<NewsEntity, NotFound>>;

public sealed class UpdateNewsHandler : IRequestHandler<UpdateNewsCommand, OneOf<NewsEntity, NotFound>>
{
    private readonly INewsDbContext _dbContext;
    private readonly ILogger<UpdateNewsHandler> _logger;

    public UpdateNewsHandler(INewsDbContext dbContext, ILogger<UpdateNewsHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async ValueTask<OneOf<NewsEntity, NotFound>> Handle(
        UpdateNewsCommand command,
        CancellationToken cancellationToken)
    {
        // TODO Validation through Behaviour Pipeline, (nick chapsas video)
        int result = await _dbContext.News.Where(x => x.ExternalId == command.Id)
            .ExecuteUpdateAsync(
                s => s
                .SetProperty(x => x.Content, command.News.Content)
                .SetProperty(x => x.Heading, command.News.Heading)
                .SetProperty(x => x.ImageUrl, command.News.ImageUrl),
                cancellationToken: cancellationToken);

        if (result == 0)
        {
            _logger.LogInformation("News to be updated with id {NewsId} not found", command.Id);
            return default(NotFound);
        }

        _logger.LogInformation("Updated News with id {NewsId}", command.Id);
        var updatedNews = await _dbContext.News.FirstAsync(x => x.ExternalId == command.Id, cancellationToken: cancellationToken);

        return updatedNews;
    }
}
