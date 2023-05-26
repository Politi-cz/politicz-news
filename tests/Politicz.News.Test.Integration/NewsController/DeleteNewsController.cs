namespace Politicz.News.Test.Integration.NewsController;

[Collection("Shared test collection")]
public class DeleteNewsController : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;

    public DeleteNewsController(NewsApiFactory apiFactory)
    {
        _resetDatabase = apiFactory.ResetDatabase;
        _client = apiFactory.HttpClient;
    }

    [Fact]
    public async Task DeleteNews_DeletesNews_WhenNewsExists()
    {
        // Arrange
        var createdNews = await Helpers.CreateNewNews(_client);

        // Act
        var response = await _client.DeleteAsync($"api/news/{createdNews.Id}");
        var responseAfterDelete = await _client.DeleteAsync($"api/news/{createdNews.Id}");

        // Assert
        _ = response.StatusCode.Should().Be(HttpStatusCode.OK);
        _ = responseAfterDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteNews_ReturnsNotFound_WhenNewsDoesNotExist()
    {
        // Act
        var response = await _client.DeleteAsync($"api/news/{Guid.NewGuid()}");

        // Assert
        _ = response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
