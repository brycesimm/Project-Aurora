using Aurora.Shared.Interfaces;
using Aurora.Shared.Models;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Aurora.Client.Core.Services;

public class ContentService : IContentService
{
    private readonly HttpClient _httpClient;

    public ContentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ContentFeed> GetDailyContentAsync()
    {
        // The BaseAddress is pre-configured in MauiProgram.cs to the API root.
        // We append the specific endpoint path here.
        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var contentFeed = await _httpClient.GetFromJsonAsync<ContentFeed>("GetDailyContent", options);
        return contentFeed ?? new ContentFeed();
    }
}
