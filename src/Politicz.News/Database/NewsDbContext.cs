namespace Politicz.News.Database;

public class NewsDbContext : DbContext, INewsDbContext
{
    private readonly DatabaseOptions _options;

    public NewsDbContext(IOptions<DatabaseOptions> options)
        => _options = options.Value;

    public DbSet<NewsEntity> News { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        base.OnConfiguring(optionsBuilder.UseSqlServer(_options.ConnectionString));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<NewsEntity>()
            .HasKey(c => c.Id);

        _ = modelBuilder.Entity<NewsEntity>()
            .HasIndex(c => c.ExternalId)
            .IsClustered(false);

        _ = modelBuilder.Entity<NewsEntity>()
            .Property(x => x.ExternalId)
            .HasDefaultValueSql("NEWID()");

        _ = modelBuilder.Entity<NewsEntity>()
            .Property(x => x.CreatedOn)
            .HasDefaultValueSql("GETUTCDATE()");

        base.OnModelCreating(modelBuilder);
    }
}
