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

		var contentFeed = await _httpClient.GetFromJsonAsync<ContentFeed>("GetDailyContent", options).ConfigureAwait(false);
		return contentFeed ?? new ContentFeed();
	}

	public async Task<int> ReactToContentAsync(string contentId)
	{
		var response = await _httpClient.PostAsync(new Uri($"articles/{contentId}/react", UriKind.Relative), null).ConfigureAwait(false);
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<ReactionResponse>().ConfigureAwait(false);
		return result?.UpliftCount ?? 0;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated via JSON deserialization")]
	private sealed record ReactionResponse(
		[property: System.Text.Json.Serialization.JsonPropertyName("uplift_count")] int UpliftCount
	);
}
