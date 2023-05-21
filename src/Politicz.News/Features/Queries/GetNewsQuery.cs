namespace Politicz.News.Features.Queries;

public sealed record GetNewsQuery(Guid Id) : IRequest<OneOf<NewsEntity, NotFound>>;

public sealed class GetNewsHandler : IRequestHandler<GetNewsQuery, OneOf<NewsEntity, NotFound>>
{
    private readonly INewsDbContext _dbContext;

    public GetNewsHandler(INewsDbContext dbContext) => _dbContext = dbContext;

    public async ValueTask<OneOf<NewsEntity, NotFound>> Handle(
        GetNewsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _dbContext.News.AsNoTracking().FirstOrDefaultAsync(
            x => x.ExternalId == request.Id,
            cancellationToken: cancellationToken);

        return result is null ? default(NotFound) : result;
    }
}
