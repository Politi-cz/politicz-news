namespace Politicz.News.Test.Integration.NewsController;

[Collection("Shared test collection")]
public class CreateNewsController : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly HttpClient _unauthorizedClient;
    private readonly Func<Task> _resetDatabase;

    public CreateNewsController(NewsApiFactory apiFactory)
    {
        _resetDatabase = apiFactory.ResetDatabase;
        _unauthorizedClient = apiFactory.UnauthorizedClient;
        _client = apiFactory.HttpClient;
    }

    [Fact]
    public async Task CreateNews_ReturnsUnauthorized_WhenUnauthorizedRequest()
    {
        // Arrange
        var newsRequest = Helpers.GetNewsRequest();

        // Act
        var newsResponse = await _unauthorizedClient.PostAsJsonAsync("api/news/create", newsRequest);

        // Assert
        _ = newsResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateNews_CreatesNews_WhenDataAreValid()
    {
        // Arrange
        var newsRequest = Helpers.GetNewsRequest();

        // Act
        var newsResponse = await _client.PostAsJsonAsync("api/news/create", newsRequest);

        // Assert
        _ = newsResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdNews = await newsResponse.Content.ReadFromJsonAsync<NewsResponse>();
        _ = createdNews.Should().BeEquivalentTo(newsRequest);
    }

    [Fact]
    public async Task CreateNews_ReturnsBadRequest_WhenDataAreInvalid()
    {
        // Arrange
        var newsRequest = Helpers.GetInvalidNewsRequest();

        // Act
        var newsResponse = await _client.PostAsJsonAsync("api/news/create", newsRequest);

        // Assert
        _ = newsResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
