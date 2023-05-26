namespace Politicz.News.Test.Integration.NewsController;

[Collection("Shared test collection")]
public class GetNewsController : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    public GetNewsController(NewsApiFactory apiFactory)
    {
        _resetDatabase = apiFactory.ResetDatabase;
        _client = apiFactory.HttpClient;
    }

    [Fact]
    public async Task GetAllNews_ReturnsAllNews()
    {
        // Arrange
        var listOfNews = new List<NewsResponse>
        {
            await Helpers.CreateNewNews(_client, "news 1"),
            await Helpers.CreateNewNews(_client, "news 2"),
            await Helpers.CreateNewNews(_client, "news 3"),
        };

        // Act
        var response = await _client.GetAsync("api/news");

        // Assert
        var returnedNews = await response.Content.ReadFromJsonAsync<IEnumerable<NewsResponse>>();
        _ = returnedNews.Should().BeEquivalentTo(listOfNews);
    }

    [Fact]
    public async Task GetNews_ReturnsNews_WhenNewsExists()
    {
        // Arrange
        var createdNews = await Helpers.CreateNewNews(_client);

        // Act
        var response = await _client.GetAsync($"api/news/{createdNews.Id}");

        // Assert
        _ = response.StatusCode.Should().Be(HttpStatusCode.OK);
        var returnedNews = await response.Content.ReadFromJsonAsync<NewsResponse>();
        _ = returnedNews.Should().BeEquivalentTo(createdNews);
    }

    [Fact]
    public async Task GetNews_ReturnsNotFound_WhenNewsDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync($"api/news/{Guid.NewGuid()}");

        // Assert
        _ = response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
