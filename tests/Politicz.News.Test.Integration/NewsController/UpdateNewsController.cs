namespace Politicz.News.Test.Integration.NewsController;

[Collection("Shared test collection")]
public class UpdateNewsController : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly HttpClient _unauthorizedClient;
    private readonly Func<Task> _resetDatabase;

    public UpdateNewsController(NewsApiFactory apiFactory)
    {
        _resetDatabase = apiFactory.ResetDatabase;
        _unauthorizedClient = apiFactory.UnauthorizedClient;
        _client = apiFactory.HttpClient;
    }

    [Fact]
    public async Task UpdateNews_ReturnsUnauthorized_WhenUnauthorizedRequest()
    {
        // Arrange
        var newsRequest = Helpers.GetNewsRequest();

        // Act
        var newsResponse = await _unauthorizedClient.PutAsJsonAsync($"api/news/{Guid.NewGuid()}", newsRequest);

        // Assert
        _ = newsResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateNews_UpdatesNews_WhenDataAreValid()
    {
        // Arrange
        var createdNewsRequest = await Helpers.CreateNewNews(_client);
        var updatedNewsRequest = createdNewsRequest with
        {
            Heading = "New updated Heading",
            Content = "updated content",
        };

        // Act
        var newsResponse = await _client.PutAsJsonAsync($"api/news/{createdNewsRequest.Id}", updatedNewsRequest);

        // Assert
        _ = newsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedNews = await newsResponse.Content.ReadFromJsonAsync<NewsResponse>();
        _ = updatedNews.Should().BeEquivalentTo(updatedNewsRequest);
    }

    [Fact]
    public async Task UpdateNews_ReturnsBadRequest_WhenDataAreInvalid()
    {
        // Arrange
        var createdNewsRequest = await Helpers.CreateNewNews(_client);
        var updatedNewsRequest = createdNewsRequest with
        {
            Heading = "New updated Heading",
            Content = string.Empty,
        };

        // Act
        var newsResponse = await _client.PutAsJsonAsync($"api/news/{createdNewsRequest.Id}", updatedNewsRequest);

        // Assert
        _ = newsResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateNews_ReturnsNotFound_WhenNewsToBeUpdatedDoesNotExist()
    {
        // Arrange
        var updatedNewsRequest = Helpers.GetNewsRequest();

        // Act
        var newsResponse = await _client.PutAsJsonAsync($"api/news/{Guid.NewGuid()}", updatedNewsRequest);

        // Assert
        _ = newsResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}
