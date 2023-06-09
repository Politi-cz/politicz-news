﻿namespace Politicz.News.Database;

public interface INewsDbContext
{
    DbSet<NewsEntity> News { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
