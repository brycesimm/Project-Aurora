using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;
using Azure.Storage.Blobs;
using Azure;
using Polly;
using Polly.Retry;

namespace Aurora.Api;

/// <summary>
/// Azure Function that retrieves daily content from Blob Storage.
/// </summary>
public class GetDailyContent(BlobServiceClient blobServiceClient, ILogger<GetDailyContent> logger)
{
	private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
	private readonly ILogger<GetDailyContent> _logger = logger;

	// Retry policy: 3 attempts with exponential backoff (1s, 2s, 4s)
	private static readonly ResiliencePipeline RetryPipeline = new ResiliencePipelineBuilder()
		.AddRetry(new RetryStrategyOptions
		{
			MaxRetryAttempts = 3,
			Delay = TimeSpan.FromSeconds(1),
			BackoffType = DelayBackoffType.Exponential,
			UseJitter = true
		})
		.Build();

	/// <summary>
	/// HTTP GET endpoint that serves the daily content feed from Azure Blob Storage.
	/// </summary>
	/// <param name="req">The HTTP request.</param>
	/// <returns>HTTP response containing the content JSON or an error message.</returns>
	[Function("GetDailyContent")]
	public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
	{
		_logger.LogInformation("GetDailyContent function invoked.");

		try
		{
			// Get configuration from environment variables
			var containerName = Environment.GetEnvironmentVariable("ContentContainerName");
			if (string.IsNullOrEmpty(containerName))
			{
				_logger.LogError("ContentContainerName environment variable is not configured.");
				var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
				await errorResponse.WriteStringAsync("Storage configuration error").ConfigureAwait(false);
				return errorResponse;
			}

			// Download content.json from blob storage with retry policy
			var jsonContent = await RetryPipeline.ExecuteAsync(async cancellationToken =>
			{
				var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
				var blobClient = containerClient.GetBlobClient("content.json");

				_logger.LogInformation("Downloading content.json from container '{ContainerName}'", containerName);

				var downloadResult = await blobClient.DownloadContentAsync(cancellationToken).ConfigureAwait(false);
				return downloadResult.Value.Content.ToString();
			}).ConfigureAwait(false);

			// Return successful response with JSON content
			var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "application/json; charset=utf-8");
			await response.WriteStringAsync(jsonContent).ConfigureAwait(false);

			_logger.LogInformation("Content successfully retrieved and returned.");
			return response;
		}
		catch (RequestFailedException ex) when (ex.Status == 404)
		{
			_logger.LogWarning("Content blob not found in storage. Status: {Status}", ex.Status);
			var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
			await notFoundResponse.WriteStringAsync("Content not yet published").ConfigureAwait(false);
			return notFoundResponse;
		}
		catch (RequestFailedException ex)
		{
			_logger.LogError(ex, "Azure Storage request failed. Status: {Status}, ErrorCode: {ErrorCode}", ex.Status, ex.ErrorCode);
			var storageErrorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
			await storageErrorResponse.WriteStringAsync("Storage configuration error").ConfigureAwait(false);
			return storageErrorResponse;
		}
#pragma warning disable CA1031 // Catch all exceptions to prevent function crashes and return consistent error responses to clients
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unexpected error retrieving content from blob storage.");
			var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
			await errorResponse.WriteStringAsync("Storage configuration error").ConfigureAwait(false);
			return errorResponse;
		}
#pragma warning restore CA1031
	}
}
