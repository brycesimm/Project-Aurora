using Aurora.Client.Core.Services;
using Aurora.Shared.Models;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Aurora.Client.Core.Tests;

public class ContentServiceTests
{
	private readonly Mock<HttpMessageHandler> _handlerMock;
	private readonly HttpClient _httpClient;
	private readonly ContentService _service;

	public ContentServiceTests()
	{
		_handlerMock = new Mock<HttpMessageHandler>();
		_httpClient = new HttpClient(_handlerMock.Object)
		{
			BaseAddress = new Uri("http://localhost/")
		};
		_service = new ContentService(_httpClient);
	}

	[Fact]
	public async Task GetDailyContentAsync_ReturnsPopulatedFeed_OnSuccess()
	{
		// Arrange
		var jsonResponse = await File.ReadAllTextAsync(Path.Combine("TestData", "sample.content.json")).ConfigureAwait(false);

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(jsonResponse)
			});

		// Act
		var result = await _service.GetDailyContentAsync().ConfigureAwait(false);

		// Assert
		Assert.NotNull(result);
		Assert.NotNull(result.VibeOfTheDay);
		// Validating against the specific data in sample.content.json
		Assert.Equal("a1b2c3d4-e5f6-7890-1234-567890abcdef", result.VibeOfTheDay.Id);
		Assert.Equal("World's First Fully Sustainable Floating City Unveiled", result.VibeOfTheDay.Title);
		Assert.Equal(1250, result.VibeOfTheDay.UpliftCount);

		Assert.NotEmpty(result.DailyPicks);
		Assert.Equal("b1c2d3e4-f5g6-7890-1234-567890abcde0", result.DailyPicks[0].Id);
		Assert.Equal(845, result.DailyPicks[0].UpliftCount);
	}

	[Fact]
	public async Task GetDailyContentAsync_ThrowsHttpRequestException_OnNotFound()
	{
		// Arrange
		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.NotFound
			});

		// Act & Assert
		await Assert.ThrowsAsync<HttpRequestException>(() => _service.GetDailyContentAsync()).ConfigureAwait(false);
	}

	[Fact]
	public async Task ReactToContentAsync_ReturnsNewCount_OnSuccess()
	{
		// Arrange
		var contentId = "test-id";
		var expectedCount = 42;

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Post &&
					req.RequestUri != null &&
					req.RequestUri.ToString().EndsWith($"articles/{contentId}/react")),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = JsonContent.Create(new { uplift_count = expectedCount })
			});

		// Act
		var result = await _service.ReactToContentAsync(contentId).ConfigureAwait(false);
		// Assert
		Assert.Equal(expectedCount, result);
	}
}
