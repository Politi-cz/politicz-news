namespace Politicz.News.Test.Integration.NewsController;

public static class Helpers
{
    public static CreateNews GetNewsRequest(string heading = "Test")
        => new(heading, "test Content", "https://images.com/test");

    public static CreateNews GetInvalidNewsRequest()
        => new(string.Empty, "test Content", "https://images.com/test");

    public static async Task<NewsResponse> CreateNewNews(HttpClient client, string heading = "Test")
    {
        var news = GetNewsRequest(heading);
        var newsResponse = await client.PostAsJsonAsync("api/news/create", news);
        return (await newsResponse.Content.ReadFromJsonAsync<NewsResponse>())!;
    }
}
