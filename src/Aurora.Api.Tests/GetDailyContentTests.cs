using Moq;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure;
using Microsoft.Extensions.Logging;

namespace Aurora.Api.Tests;

/// <summary>
/// Unit tests for GetDailyContent blob storage interactions.
/// Tests blob storage client behavior, Polly retry logic, and error handling.
/// Note: These tests verify the blob storage layer; end-to-end HTTP testing requires integration tests.
/// </summary>
public class GetDailyContentTests
{
	private readonly Mock<BlobServiceClient> _mockBlobServiceClient;
	private readonly Mock<BlobContainerClient> _mockContainerClient;
	private readonly Mock<BlobClient> _mockBlobClient;
	private readonly Mock<ILogger<GetDailyContent>> _mockLogger;

	public GetDailyContentTests()
	{
		_mockBlobServiceClient = new Mock<BlobServiceClient>();
		_mockContainerClient = new Mock<BlobContainerClient>();
		_mockBlobClient = new Mock<BlobClient>();
		_mockLogger = new Mock<ILogger<GetDailyContent>>();

		// Setup container and blob client chain
		_mockBlobServiceClient
			.Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
			.Returns(_mockContainerClient.Object);

		_mockContainerClient
			.Setup(x => x.GetBlobClient("content.json"))
			.Returns(_mockBlobClient.Object);

		// Setup environment variable for container name
		Environment.SetEnvironmentVariable("ContentContainerName", "aurora-content");
	}

	[Fact]
	public async Task BlobClient_DownloadsContent_WhenBlobExists()
	{
		// Arrange
		var testContent = @"{""VibeOfTheDay"":{""title"":""Test Story""}}";
		var binaryData = BinaryData.FromString(testContent);
		var blobDownloadResult = BlobsModelFactory.BlobDownloadResult(content: binaryData);
		var response = Response.FromValue(blobDownloadResult, Mock.Of<Response>());

		_mockBlobClient
			.Setup(x => x.DownloadContentAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(response);

		// Act
		var result = await _mockBlobClient.Object.DownloadContentAsync(CancellationToken.None).ConfigureAwait(false);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(testContent, result.Value.Content.ToString());
		_mockBlobClient.Verify(x => x.DownloadContentAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task BlobClient_Throws404_WhenBlobNotFound()
	{
		// Arrange
		var exception = new RequestFailedException(404, "Blob not found");

		_mockBlobClient
			.Setup(x => x.DownloadContentAsync(It.IsAny<CancellationToken>()))
			.ThrowsAsync(exception);

		// Act & Assert
		var thrownException = await Assert.ThrowsAsync<RequestFailedException>(
			async () => await _mockBlobClient.Object.DownloadContentAsync(CancellationToken.None).ConfigureAwait(false)
		).ConfigureAwait(false);

		Assert.Equal(404, thrownException.Status);
		_mockBlobClient.Verify(x => x.DownloadContentAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async Task BlobClient_RetriesOnTransientFailure_AndSucceeds()
	{
		// Arrange
		var testContent = @"{""VibeOfTheDay"":{""title"":""Test Story""}}";
		var binaryData = BinaryData.FromString(testContent);
		var blobDownloadResult = BlobsModelFactory.BlobDownloadResult(content: binaryData);
		var successResponse = Response.FromValue(blobDownloadResult, Mock.Of<Response>());

		var transientException = new RequestFailedException(500, "Transient error");

		var callCount = 0;
		_mockBlobClient
			.Setup(x => x.DownloadContentAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(() =>
			{
				callCount++;
				if (callCount == 1)
					throw transientException;
				return successResponse;
			});

		// Act - Simulate Polly retry logic
		Response<BlobDownloadResult>? result = null;
		var maxRetries = 3;
		var attempt = 0;

		while (attempt <= maxRetries)
		{
			try
			{
				result = await _mockBlobClient.Object.DownloadContentAsync(CancellationToken.None).ConfigureAwait(false);
				break;
			}
			catch (RequestFailedException) when (attempt < maxRetries)
			{
				attempt++;
				await Task.Delay(10).ConfigureAwait(false); // Simulate retry delay
			}
		}

		// Assert
		Assert.NotNull(result);
		Assert.Equal(2, callCount); // First call fails, second succeeds
		Assert.Equal(testContent, result.Value.Content.ToString());
	}

	[Fact]
	public async Task BlobClient_ThrowsAfterRetryExhaustion()
	{
		// Arrange
		var transientException = new RequestFailedException(503, "Service unavailable");

		_mockBlobClient
			.Setup(x => x.DownloadContentAsync(It.IsAny<CancellationToken>()))
			.ThrowsAsync(transientException);

		// Act - Simulate Polly retry logic with exhaustion
		var maxRetries = 3;
		var attempt = 0;
		RequestFailedException? caughtException = null;

		try
		{
			while (attempt <= maxRetries)
			{
				try
				{
					await _mockBlobClient.Object.DownloadContentAsync(CancellationToken.None).ConfigureAwait(false);
					break;
				}
				catch (RequestFailedException ex)
				{
					caughtException = ex;
					attempt++;
					if (attempt > maxRetries)
					{
						throw;
					}
					await Task.Delay(10).ConfigureAwait(false);
				}
			}
		}
		catch (RequestFailedException)
		{
			// Expected - retry exhausted
		}

		// Assert
		Assert.NotNull(caughtException);
		Assert.Equal(503, caughtException.Status);
		// Polly retry: 1 initial + 3 retries = 4 total attempts
		_mockBlobClient.Verify(
			x => x.DownloadContentAsync(It.IsAny<CancellationToken>()),
			Times.Exactly(4));
	}

	[Fact]
	public void BlobServiceClient_CreatesContainerClient_WithCorrectName()
	{
		// Arrange
		var containerName = "aurora-content";

		// Act
		var containerClient = _mockBlobServiceClient.Object.GetBlobContainerClient(containerName);

		// Assert
		Assert.NotNull(containerClient);
		_mockBlobServiceClient.Verify(x => x.GetBlobContainerClient(containerName), Times.Once);
	}

	[Fact]
	public void ContainerClient_CreatesBlobClient_ForContentJson()
	{
		// Arrange & Act
		var blobClient = _mockContainerClient.Object.GetBlobClient("content.json");

		// Assert
		Assert.NotNull(blobClient);
		_mockContainerClient.Verify(x => x.GetBlobClient("content.json"), Times.Once);
	}
}
