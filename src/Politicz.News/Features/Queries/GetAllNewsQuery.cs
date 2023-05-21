namespace Politicz.News.Features.Queries;

public sealed record GetAllNewsQuery : IRequest<IEnumerable<NewsEntity>>;

public sealed class GetAllNewsHandler : IRequestHandler<GetAllNewsQuery, IEnumerable<NewsEntity>>
{
    private readonly INewsDbContext _dbContext;

    public GetAllNewsHandler(INewsDbContext dbContext) => _dbContext = dbContext;

    public async ValueTask<IEnumerable<NewsEntity>> Handle(
        GetAllNewsQuery request,
        CancellationToken cancellationToken)
        => await _dbContext.News.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
}
