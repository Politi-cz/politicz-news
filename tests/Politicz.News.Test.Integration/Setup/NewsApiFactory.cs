namespace Politicz.News.Test.Integration.Setup;

public class NewsApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("noveHeslo1")
        .WithCleanUp(true)
        .Build();

    private string _connectionString = default!;
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = default!;

    public HttpClient UnauthorizedClient { get; private set; } = default!;

    public async Task ResetDatabase() => await _respawner.ResetAsync(_dbConnection);

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        var connBuilder = new SqlConnectionStringBuilder(_dbContainer.GetConnectionString())
        {
            TrustServerCertificate = true,
            InitialCatalog = "politicz-news",
        };
        _connectionString = connBuilder.ConnectionString;
        _dbConnection = new SqlConnection(_connectionString);
        UnauthorizedClient = CreateClient();
        HttpClient = CreateClient();
        HttpClient.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue(scheme: TestAuthenticationHandler.AuthenticationScheme);

        await InitializeRespawner();
    }

    async Task IAsyncLifetime.DisposeAsync() => await _dbContainer.DisposeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _ = builder.ConfigureLogging(c => c.ClearProviders());
        _ = builder.ConfigureTestServices(services
            => services
                .AddTestDatabase(_connectionString)
                .AddTestSecurity());
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.SqlServer,
                SchemasToInclude = new[] { "dbo" },
            });
    }
}
