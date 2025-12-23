using Moq;
using Azure.Data.Tables;
using Azure;
using Aurora.Api.Services;
using Aurora.Api.Models;

namespace Aurora.Api.Tests;

public class ReactionStorageServiceTests
{
	private readonly Mock<TableServiceClient> _mockServiceClient;
	private readonly Mock<TableClient> _mockTableClient;
	private readonly ReactionStorageService _service;

	public ReactionStorageServiceTests()
	{
		_mockServiceClient = new Mock<TableServiceClient>();
		_mockTableClient = new Mock<TableClient>();

		_mockServiceClient.Setup(x => x.GetTableClient(It.IsAny<string>()))
			.Returns(_mockTableClient.Object);

		_service = new ReactionStorageService(_mockServiceClient.Object);
	}

	[Fact]
	public async Task GetUpliftCountAsync_ReturnsCount_WhenEntityExists()
	{
		// Arrange
		var articleId = "test-article";
		var expectedCount = 5;
		var entity = new ReactionEntity { RowKey = articleId, UpliftCount = expectedCount };

		var mockResponse = Response.FromValue(entity, Mock.Of<Response>());

		_mockTableClient.Setup(x => x.GetEntityAsync<ReactionEntity>(
			It.IsAny<string>(),
			articleId,
			It.IsAny<IEnumerable<string>>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(mockResponse);

		// Act
		var count = await _service.GetUpliftCountAsync(articleId).ConfigureAwait(false);

		// Assert
		Assert.Equal(expectedCount, count);
	}

	[Fact]
	public async Task GetUpliftCountAsync_ReturnsZero_WhenEntityNotFound()
	{
		// Arrange
		var articleId = "missing-article";

		var exception = new RequestFailedException(404, "Not Found");

		_mockTableClient.Setup(x => x.GetEntityAsync<ReactionEntity>(
			It.IsAny<string>(),
			articleId,
			It.IsAny<IEnumerable<string>>(),
			It.IsAny<CancellationToken>()))
			.ThrowsAsync(exception);

		// Act
		var count = await _service.GetUpliftCountAsync(articleId).ConfigureAwait(false);

		// Assert
		Assert.Equal(0, count);
	}

	[Fact]
	public async Task IncrementUpliftCountAsync_Increments_WhenEntityExists()
	{
		// Arrange
		var articleId = "existing-article";
		var initialCount = 10;
		var entity = new ReactionEntity { RowKey = articleId, UpliftCount = initialCount };

		var mockGetResponse = Response.FromValue(entity, Mock.Of<Response>());

		_mockTableClient.Setup(x => x.GetEntityAsync<ReactionEntity>(
			It.IsAny<string>(),
			articleId,
			It.IsAny<IEnumerable<string>>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(mockGetResponse);

		_mockTableClient.Setup(x => x.UpsertEntityAsync(
			It.IsAny<ReactionEntity>(),
			It.IsAny<TableUpdateMode>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(Mock.Of<Response>());

		// Act
		var newCount = await _service.IncrementUpliftCountAsync(articleId).ConfigureAwait(false);

		// Assert
		Assert.Equal(initialCount + 1, newCount);
		_mockTableClient.Verify(x => x.UpsertEntityAsync(
			It.Is<ReactionEntity>(e => e.UpliftCount == initialCount + 1 && e.RowKey == articleId),
			It.IsAny<TableUpdateMode>(),
			It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task IncrementUpliftCountAsync_CreatesNew_WhenEntityNotFound()
	{
		// Arrange
		var articleId = "new-article";
		var exception = new RequestFailedException(404, "Not Found");

		_mockTableClient.Setup(x => x.GetEntityAsync<ReactionEntity>(
			It.IsAny<string>(),
			articleId,
			It.IsAny<IEnumerable<string>>(),
			It.IsAny<CancellationToken>()))
			.ThrowsAsync(exception);

		_mockTableClient.Setup(x => x.UpsertEntityAsync(
			It.IsAny<ReactionEntity>(),
			It.IsAny<TableUpdateMode>(),
			It.IsAny<CancellationToken>()))
			.ReturnsAsync(Mock.Of<Response>());

		// Act
		var newCount = await _service.IncrementUpliftCountAsync(articleId).ConfigureAwait(false);

		// Assert
		Assert.Equal(1, newCount);
		_mockTableClient.Verify(x => x.UpsertEntityAsync(
			It.Is<ReactionEntity>(e => e.UpliftCount == 1 && e.RowKey == articleId),
			It.IsAny<TableUpdateMode>(),
			It.IsAny<CancellationToken>()), Times.Once);
	}
}
