using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;
using System.Reflection;

namespace Aurora.Api;

public class GetDailyContent
{
	private readonly ILogger<GetDailyContent> _logger;

	public GetDailyContent(ILogger<GetDailyContent> logger)
	{
		_logger = logger;
	}

	[Function("GetDailyContent")]
	public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
	{
		_logger.LogInformation("C# HTTP trigger function processed a request.");

		var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
		response.Headers.Add("Content-Type", "application/json; charset=utf-8");

		var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		if (exePath is null)
		{
			_logger.LogError("Could not determine the execution path.");
			return req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
		}
		var contentPath = Path.Combine(exePath, "sample.content.json");

		if (!File.Exists(contentPath))
		{
			_logger.LogError("sample.content.json not found at {Path}", contentPath);
			return req.CreateResponse(System.Net.HttpStatusCode.NotFound);
		}

		var jsonContent = await File.ReadAllTextAsync(contentPath).ConfigureAwait(false);
		await response.WriteStringAsync(jsonContent).ConfigureAwait(false);

		return response;
	}
}
