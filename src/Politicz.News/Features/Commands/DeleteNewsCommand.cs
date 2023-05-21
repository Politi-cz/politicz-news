namespace Politicz.News.Features.Commands;

public sealed record DeleteNewsCommand(Guid Id) : IRequest<OneOf<Success, NotFound>>;

public sealed class DeleteNewsHandler : IRequestHandler<DeleteNewsCommand, OneOf<Success, NotFound>>
{
    private readonly INewsDbContext _dbContext;
    private readonly ILogger<DeleteNewsHandler> _logger;

    public DeleteNewsHandler(INewsDbContext dbContext, ILogger<DeleteNewsHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async ValueTask<OneOf<Success, NotFound>> Handle(
        DeleteNewsCommand command,
        CancellationToken cancellationToken)
    {
        int result = await _dbContext.News.Where(x => x.ExternalId == command.Id)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);

        if (result == 0)
        {
            _logger.LogInformation("News to be deleted with id {NewsId} not found", command.Id);
            return default(NotFound);
        }

        _logger.LogInformation("Deleted News with id {NewsId}", command.Id);
        return default(Success);
    }
}
