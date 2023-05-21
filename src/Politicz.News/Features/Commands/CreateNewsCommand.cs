namespace Politicz.News.Features.Commands;

public sealed record CreateNewsCommand(CreateNews NewsDto) : IRequest<NewsEntity>;

public sealed class CreateNewsHandler : IRequestHandler<CreateNewsCommand, NewsEntity>
{
    private readonly INewsDbContext _dbContext;
    private readonly ILogger<CreateNewsHandler> _logger;

    public CreateNewsHandler(INewsDbContext dbContext, ILogger<CreateNewsHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async ValueTask<NewsEntity> Handle(
        CreateNewsCommand command,
        CancellationToken cancellationToken)
    {
        // TODO Validation through Behaviour Pipeline, (nick chapsas video)
        var newsEntity = command.NewsDto.ToEntity();

        var result = (await _dbContext.News.AddAsync(newsEntity, cancellationToken)).Entity;
        _ = await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created new News with id {NewsId}", result.ExternalId);
        return result;
    }
}
