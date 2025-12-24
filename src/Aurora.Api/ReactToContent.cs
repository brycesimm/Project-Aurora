using System.Net;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Aurora.Api.Services;

namespace Aurora.Api;

public class ReactToContent
{
	private readonly ILogger<ReactToContent> _logger;
	private readonly ReactionStorageService _reactionService;

	public ReactToContent(ILogger<ReactToContent> logger, ReactionStorageService reactionService)
	{
		_logger = logger;
		_reactionService = reactionService;
	}

	[Function("ReactToContent")]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Catching all exceptions at the API boundary to return a 500 status code.")]
	public async Task<HttpResponseData> Run(
		[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "articles/{id}/react")] HttpRequestData req,
		string id)
	{
		_logger.LogInformation("Processing reaction for article: {ArticleId}", id);

		if (string.IsNullOrWhiteSpace(id))
		{
			return req.CreateResponse(HttpStatusCode.BadRequest);
		}

		try
		{
			var newCount = await _reactionService.IncrementUpliftCountAsync(id).ConfigureAwait(false);

			var response = req.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "application/json; charset=utf-8");

			var jsonResponse = $"{{\"uplift_count\": {newCount}}}";
			await response.WriteStringAsync(jsonResponse).ConfigureAwait(false);

			return response;
		}
		catch (Azure.RequestFailedException ex)
		{
			_logger.LogError(ex, "Azure Storage error processing reaction for article: {ArticleId}", id);
			return req.CreateResponse(HttpStatusCode.InternalServerError);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unexpected error processing reaction for article: {ArticleId}", id);
			return req.CreateResponse(HttpStatusCode.InternalServerError);
		}
	}
}
